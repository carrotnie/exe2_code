using Influencerhub.Common.DTO;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IInfluService
    {
        Task<ResponseDTO> CreateInflu(InfluDTO influDto);

        Task<ResponseDTO> UpdateInfluByUserId(Guid userId, InfluUpdateDTO influDto);

        Task<ResponseDTO> SearchByName(string keyword);
        Task<ResponseDTO> SearchByFieldName(string keyword);
        Task<ResponseDTO> SearchByArea(string keyword);
        Task<ResponseDTO> SearchByFollower(int? minFollower, int? maxFollower);
        Task<ResponseDTO> GetAllInflu();
    }
}
