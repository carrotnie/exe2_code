using Microsoft.AspNetCore.Mvc;
using Influencerhub.Services.Contract;
using Influencerhub.Common.DTO;
using System;
using System.Threading.Tasks;

namespace Influencerhub.API.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("business-review-influ")]
        public async Task<ResponseDTO> BusinessReviewInflu([FromBody] BusinessReviewCreateDTO dto)
        {
            return await _reviewService.BusinessReviewInflu(dto.JobId, dto.Feedback, dto.Rating);
        }

        [HttpPost("influ-review-business")]
        public async Task<ResponseDTO> InfluReviewBusiness([FromBody] InfluReviewCreateDTO dto)
        {
            return await _reviewService.InfluReviewBusiness(dto.FreelanceJobId, dto.Feedback, dto.Rating);
        }

        [HttpGet("review-of-influ/{userId}")]
        public async Task<ResponseDTO> GetReviewOfInflu([FromRoute] Guid userId)
        {
            return await _reviewService.GetReviewOfInflu(userId);
        }

        [HttpGet("review-of-business/{userId}")]
        public async Task<ResponseDTO> GetReviewOfBusiness([FromRoute] Guid userId)
        {
            return await _reviewService.GetReviewOfBusiness(userId);
        }

        // Lấy tất cả rating (không kiểm tra membership)
        [HttpGet("rating-list")]
        public async Task<ResponseDTO> GetRatingList([FromRoute] Guid userId, [FromQuery] bool isBusiness)
        {
            return await _reviewService.GetRatingList(userId, isBusiness);
        }
    }
}
