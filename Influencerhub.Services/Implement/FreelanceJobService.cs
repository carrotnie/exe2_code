using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Influencerhub.Services.Implement
{
    public class FreelanceJobService : IFreelanceJobService
    {
        private readonly IFreelanceJobRepository _freelanceJobRepository;
        private readonly IJobRepository _jobRepository;
        private readonly InfluencerhubDBContext _context;

        public FreelanceJobService(
            IFreelanceJobRepository freelanceJobRepository,
            IJobRepository jobRepository,
            InfluencerhubDBContext context)
        {
            _freelanceJobRepository = freelanceJobRepository;
            _jobRepository = jobRepository;
            _context = context;
        }

        public async Task<ResponseDTO> ApplyForJob(Guid jobId, Guid freelanceId)
        {
            var response = new ResponseDTO();
            try
            {
                // Check đã apply chưa
                var existed = await _freelanceJobRepository.GetByJobAndFreelance(jobId, freelanceId);
                if (existed != null)
                {
                    response.IsSuccess = false;
                    response.Message = "Bạn đã apply công việc này rồi!";
                    return response;
                }

                // Lấy thông tin job
                var job = await _jobRepository.GetJobById(jobId);
                if (job == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Job không tồn tại";
                    return response;
                }

                // Chỉ cho apply khi Job còn Available
                if (job.Status != JobStatus.Available)
                {
                    response.IsSuccess = false;
                    response.Message = "Chỉ có thể apply vào Job còn mở đăng ký";
                    return response;
                }

                var entity = new FreelanceJob
                {
                    Id = Guid.NewGuid(),
                    JobId = jobId,
                    FreelanceId = freelanceId,
                    status = FreelanceJobStatus.NotYetConfirmed,
                    StartTime = job.StartTime,
                    EndTime = job.EndTime,
                    CancelTime = null
                };

                var result = await _freelanceJobRepository.Add(entity);
                response.Data = result;
                response.IsSuccess = true;
                response.Message = "Apply job thành công";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> GetApplicantsByJob(Guid jobId)
        {
            var response = new ResponseDTO();
            try
            {
                var applicants = await _freelanceJobRepository.GetByJobId(jobId);

                // Lấy tất cả InfluId (FreelanceId)
                var allFreelanceIds = applicants.Select(a => a.FreelanceId).Distinct().ToList();

                // Lấy danh sách những UserId còn membership
                var now = DateTime.UtcNow;
                var membershipUserIds = await _context.Memberships
                    .Where(m => m.UserId != null && m.EndDate > now)
                    .Select(m => m.UserId.Value)
                    .Distinct()
                    .ToListAsync();

                // Lấy list Influ để map InfluId <-> UserId
                var influList = await _context.Influs
                    .Where(i => allFreelanceIds.Contains(i.InfluId))
                    .Select(i => new { i.InfluId, i.UserId, i.Name, i.LinkImage, i.Follower })
                    .ToListAsync();

                // InfluId nào là thành viên
                var memberInfluIds = influList
                    .Where(i => i.UserId.HasValue && membershipUserIds.Contains(i.UserId.Value))
                    .Select(i => i.InfluId)
                    .ToList();

                // Group 1: KOL có membership, Group 2: không có
                var group1 = applicants
                    .Where(a => memberInfluIds.Contains(a.FreelanceId ?? Guid.Empty))
                    .OrderBy(a => a.StartTime ?? DateTime.MinValue)
                    .ToList();

                var group2 = applicants
                    .Where(a => !memberInfluIds.Contains(a.FreelanceId ?? Guid.Empty))
                    .OrderBy(a => a.StartTime ?? DateTime.MinValue)
                    .ToList();

                // Trả về kèm info Influ cho FE hiển thị đẹp (Name, Avatar, Follower,...)
                var result = group1.Concat(group2)
                    .Select(a =>
                    {
                        var influ = influList.FirstOrDefault(i => i.InfluId == a.FreelanceId);
                        return new
                        {
                            a.Id,
                            a.FreelanceId,
                            InfluName = influ?.Name,
                            InfluAvatar = influ?.LinkImage,
                            InfluFollower = influ?.Follower,
                            HasMembership = influ != null && influ.UserId.HasValue && membershipUserIds.Contains(influ.UserId.Value),
                            a.StartTime,
                            a.status
                        };
                    })
                    .ToList();

                response.Data = result;
                response.IsSuccess = true;
                response.Message = "Lấy danh sách Influencer đã apply thành công";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> ApproveInfluencerForJob(Guid freelanceJobId)
        {
            var response = new ResponseDTO();
            try
            {
                // 1. Lấy đơn ứng tuyển được duyệt
                var approvedApplication = await _freelanceJobRepository.GetById(freelanceJobId);
                if (approvedApplication == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Không tìm thấy đơn ứng tuyển này";
                    return response;
                }

                // 2. Lấy tất cả ứng viên của job này
                var allApplications = await _freelanceJobRepository.GetByJobId(approvedApplication.JobId.Value);

                foreach (var app in allApplications)
                {
                    if (app.Id == freelanceJobId)
                    {
                        app.status = FreelanceJobStatus.InProgress;
                    }
                    else
                    {
                        app.status = FreelanceJobStatus.Cancel;
                        app.CancelTime = DateTime.UtcNow;
                    }
                    await _freelanceJobRepository.Update(app);
                }

                // 3. Tự động cập nhật status của job
                var statusResult = await _jobRepository.UpdateJobStatus(approvedApplication.JobId.Value, JobStatus.InProgress);

                response.IsSuccess = true;
                response.Message = statusResult
                    ? "Duyệt ứng viên thành công, các ứng viên khác đã bị huỷ"
                    : "Duyệt ứng viên thành công, nhưng cập nhật trạng thái job không thành công";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseDTO> ConfirmCompleteJob(Guid freelanceJobId)
        {
            var response = new ResponseDTO();
            try
            {
                // 1. Lấy đơn ứng tuyển được xác nhận hoàn thành
                var freelanceJob = await _freelanceJobRepository.GetById(freelanceJobId);
                if (freelanceJob == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Không tìm thấy đơn ứng tuyển này";
                    return response;
                }

                // 2. Cập nhật status FreelanceJob thành Complete
                freelanceJob.status = FreelanceJobStatus.Complete;
                await _freelanceJobRepository.Update(freelanceJob);

                // 3. Cập nhật status của Job thành Complete
                if (freelanceJob.JobId.HasValue)
                {
                    await _jobRepository.UpdateJobStatus(freelanceJob.JobId.Value, JobStatus.Complete);
                }

                response.IsSuccess = true;
                response.Message = "Xác nhận hoàn thành công việc thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
