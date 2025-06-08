using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;

    public JobService(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<ResponseDTO> GetAll()
    {
        var response = new ResponseDTO();
        try
        {
            response.Data = await _jobRepository.GetAll();
            response.IsSuccess = true;
            response.Message = "Lấy danh sách Job thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> Add(JobDTO jobDto)
    {
        var response = new ResponseDTO();
        try
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                BusinessId = jobDto.BusinessId,
                BusinessFieldId = jobDto.BusinessFieldId,
                Title = jobDto.Title,
                Description = jobDto.Description,
                Location = jobDto.Location,
                Budget = jobDto.Budget,
                StartTime = jobDto.StartTime,
                EndTime = jobDto.EndTime,
                Require = jobDto.Require,
                Status = jobDto.Status,
                KolBenefits = jobDto.KolBenefits,
                Gender = jobDto.Gender,
                Follower = jobDto.Follower
            };


            response.Data = await _jobRepository.Add(job);
            response.IsSuccess = true;
            response.Message = "Thêm Job thành công";
        }
        catch (DbUpdateException dbEx)
        {
            var innerMsg = dbEx.InnerException?.Message ?? dbEx.Message;
            response.IsSuccess = false;
            response.Message = "Lỗi khi lưu vào database: " + innerMsg;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Lỗi không xác định: " + ex.Message;
        }
        return response;
    }


    public async Task<ResponseDTO> Update(Guid id, UpdateJobDTO jobDto)
    {
        var response = new ResponseDTO();
        try
        {
            var updatedJob = await _jobRepository.Update(id, jobDto);
            response.Data = updatedJob;
            response.IsSuccess = true;
            response.Message = "Cập nhật Job thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> Delete(Guid id)
    {
        var response = new ResponseDTO();
        try
        {
            var result = await _jobRepository.Delete(id);
            response.IsSuccess = result;
            response.Message = result ? "Xóa Job thành công" : "Không tìm thấy Job";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> GetJobById(Guid id)
    {
        var response = new ResponseDTO();
        try
        {
            var job = await _jobRepository.GetJobById(id);
            response.Data = job;
            response.IsSuccess = job != null;
            response.Message = job != null ? "Lấy Job thành công" : "Không tìm thấy Job";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> GetJobsByBusinessName(string businessName)
    {
        var response = new ResponseDTO();
        try
        {
            response.Data = await _jobRepository.GetJobsByBusinessName(businessName);
            response.IsSuccess = true;
            response.Message = "Lấy Job theo tên doanh nghiệp thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> GetJobsByBusinessField(Guid businessFieldId)
    {
        var response = new ResponseDTO();
        try
        {
            response.Data = await _jobRepository.GetJobsByBusinessField(businessFieldId);
            response.IsSuccess = true;
            response.Message = "Lấy Job theo field thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> FilterJobsByLocation(string location)
    {
        var response = new ResponseDTO();
        try
        {
            response.Data = await _jobRepository.FilterJobsByLocation(location);
            response.IsSuccess = true;
            response.Message = "Lọc Job theo vị trí thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> FilterJobsByBudget(decimal minBudget, decimal maxBudget)
    {
        var response = new ResponseDTO();
        try
        {
            response.Data = await _jobRepository.FilterJobsByBudget(minBudget, maxBudget);
            response.IsSuccess = true;
            response.Message = "Lọc Job theo budget thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> GetJobsByFieldName(string fieldName)
    {
        var response = new ResponseDTO();
        try
        {
            response.Data = await _jobRepository.GetJobsByFieldName(fieldName);
            response.IsSuccess = true;
            response.Message = "Lấy Job theo tên Field thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> FilterJobsByStartDate(DateTime fromDate)
    {
        var response = new ResponseDTO();
        try
        {
            response.Data = await _jobRepository.FilterJobsByStartDate(fromDate);
            response.IsSuccess = true;
            response.Message = "Lọc Job theo ngày bắt đầu thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }


    public async Task<ResponseDTO> UpdateJobStatus(Guid id, JobStatus newStatus)
    {
        var response = new ResponseDTO();
        try
        {
            var success = await _jobRepository.UpdateJobStatus(id, newStatus);
            response.IsSuccess = success;
            response.Message = success ? "Cập nhật trạng thái Job thành công" : "Không tìm thấy Job";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> GetJobsByBusinessId(Guid businessId)
    {
        var response = new ResponseDTO();
        try
        {
            var jobs = await _jobRepository.GetJobsByBusinessId(businessId);
            response.Data = jobs;
            response.IsSuccess = true;
            response.Message = "Lấy danh sách Job theo Business thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> GetJobsByStatus(JobStatus status)
    {
        var response = new ResponseDTO();
        try
        {
            var jobs = await _jobRepository.GetJobsByStatus(status);
            response.Data = jobs;
            response.IsSuccess = true;
            response.Message = "Lấy danh sách Job theo Status thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> GetJobsByStatusAndBusinessId(JobStatus status, Guid businessId)
    {
        var response = new ResponseDTO();
        try
        {
            var jobs = await _jobRepository.GetJobsByStatusAndBusinessId(status, businessId);
            response.Data = jobs;
            response.IsSuccess = true;
            response.Message = "Lấy danh sách Job theo Status và Business thành công";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }


}
