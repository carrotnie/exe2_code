using Influencerhub.Common.DTO;
using System.Threading.Tasks;
using System;

public interface IReviewService
{
    Task<ResponseDTO> BusinessReviewInflu(Guid jobId, string feedback, float rating);
    Task<ResponseDTO> InfluReviewBusiness(Guid freelanceJobId, string feedback, float rating);
    Task<ResponseDTO> GetReviewOfInflu(Guid userId);
    Task<ResponseDTO> GetReviewOfBusiness(Guid userId);
    Task<ResponseDTO> GetRatingOfAllBusiness();
    Task<ResponseDTO> GetRatingOfAllInflu();

}
