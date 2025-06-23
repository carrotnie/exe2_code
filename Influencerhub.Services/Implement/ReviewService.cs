using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;
using Microsoft.EntityFrameworkCore;

public class ReviewService : IReviewService
{
    private readonly InfluencerhubDBContext _context;
    private readonly IReviewRepository _reviewRepo;

    public ReviewService(InfluencerhubDBContext context, IReviewRepository reviewRepo)
    {
        _context = context;
        _reviewRepo = reviewRepo;
    }

    // Business đánh giá Influ (chỉ cho phép khi Job.Complete)
    public async Task<ResponseDTO> BusinessReviewInflu(Guid jobId, string feedback, float rating)
    {
        var response = new ResponseDTO();
        try
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null || job.Status != JobStatus.Complete)
                throw new Exception("Chỉ có thể đánh giá khi dự án đã hoàn thành!");

            var freelanceJob = await _context.FreelanceJobs
                .FirstOrDefaultAsync(f => f.JobId == jobId && f.status == FreelanceJobStatus.Complete);

            if (freelanceJob == null)
                throw new Exception("Không tìm thấy Influencer đã hoàn thành job này!");

            var review = new Review
            {
                Id = Guid.NewGuid(),
                JobId = jobId,
                InfluId = freelanceJob.FreelanceId,
                BusinessId = job.BusinessId,
                Feedback = feedback,
                Rating = rating,
                Type = ReviewType.BusinessToInflu
            };
            await _reviewRepo.AddAsync(review);

            response.IsSuccess = true;
            response.Message = "Đánh giá KOL thành công!";
            response.Data = review;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Đánh giá thất bại: " + ex.Message;
        }
        return response;
    }

    // Influ đánh giá Business (chỉ cho phép khi FreelanceJob.Complete)
    public async Task<ResponseDTO> InfluReviewBusiness(Guid freelanceJobId, string feedback, float rating)
    {
        var response = new ResponseDTO();
        try
        {
            var freelanceJob = await _context.FreelanceJobs
                .Include(fj => fj.Job)
                .FirstOrDefaultAsync(fj => fj.Id == freelanceJobId);

            if (freelanceJob == null || freelanceJob.status != FreelanceJobStatus.Complete)
                throw new Exception("Chỉ có thể đánh giá khi job đã hoàn thành!");

            var review = new Review
            {
                Id = Guid.NewGuid(),
                JobId = freelanceJob.JobId,
                InfluId = freelanceJob.FreelanceId,
                BusinessId = freelanceJob.Job?.BusinessId,
                Feedback = feedback,
                Rating = rating,
                Type = ReviewType.InfluToBusiness
            };
            await _reviewRepo.AddAsync(review);

            response.IsSuccess = true;
            response.Message = "Đánh giá business thành công!";
            response.Data = review;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Đánh giá thất bại: " + ex.Message;
        }
        return response;
    }

    // Get review của Influ
    public async Task<ResponseDTO> GetReviewOfInflu(Guid userId)
    {
        var response = new ResponseDTO();
        try
        {
            // 1. Kiểm tra membership của người gọi
            var membership = await _context.Memberships
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.EndDate)
                .FirstOrDefaultAsync();

            if (membership == null)
                throw new Exception("User chưa đăng ký gói thành viên.");

            if (membership.EndDate <= DateTime.UtcNow)
                throw new Exception($"Gói thành viên đã hết hạn từ {membership.EndDate:dd/MM/yyyy HH:mm:ss}.");

            // 2. Lấy tất cả feedback từ các review dạng Business -> Influ
            var reviews = await _context.Reviews
                .Where(r => r.Type == ReviewType.BusinessToInflu)
                .ToListAsync();

            response.IsSuccess = true;
            response.Data = reviews;
            response.Message = "Lấy tất cả review của KOL thành công!";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Không thể hiển thị review: " + ex.Message;
        }
        return response;
    }





    public async Task<ResponseDTO> GetReviewOfBusiness(Guid userId)
    {
        var response = new ResponseDTO();
        try
        {
            // 1. Kiểm tra membership của người gọi
            var membership = await _context.Memberships
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.EndDate)
                .FirstOrDefaultAsync();

            if (membership == null)
                throw new Exception("User chưa đăng ký gói thành viên.");

            if (membership.EndDate <= DateTime.UtcNow)
                throw new Exception($"Gói thành viên đã hết hạn từ {membership.EndDate:dd/MM/yyyy HH:mm:ss}.");

            // 2. Lấy tất cả feedback từ các review dạng Influ -> Business
            var reviews = await _context.Reviews
                .Where(r => r.Type == ReviewType.InfluToBusiness)
                .ToListAsync();

            response.IsSuccess = true;
            response.Data = reviews;
            response.Message = "Lấy tất cả review của business thành công!";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Không thể hiển thị review: " + ex.Message;
        }
        return response;
    }


    public async Task<ResponseDTO> GetRatingOfAllBusiness()
    {
        var response = new ResponseDTO();
        try
        {
            var reviews = await _context.Reviews
                .Where(r => r.Type == ReviewType.InfluToBusiness)
                .ToListAsync();

            var ratingList = reviews.Select(r => new
            {
                r.Id,
                r.JobId,
                r.Feedback,
                r.Rating,
                r.Type
            }).ToList();

            response.IsSuccess = true;
            response.Data = ratingList;
            response.Message = "Lấy danh sách rating của tất cả business thành công!";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDTO> GetRatingOfAllInflu()
    {
        var response = new ResponseDTO();
        try
        {
            var reviews = await _context.Reviews
                .Where(r => r.Type == ReviewType.BusinessToInflu)
                .ToListAsync();

            var ratingList = reviews.Select(r => new
            {
                r.Id,
                r.JobId,
                r.Feedback,
                r.Rating,
                r.Type
            }).ToList();

            response.IsSuccess = true;
            response.Data = ratingList;
            response.Message = "Lấy danh sách rating của tất cả influencer thành công!";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }




}
