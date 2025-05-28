using Influencerhub.Common.DTO;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IInfluService
    {
        Task<ResponseDTO> CreateInflu(InfluDTO influDto);

        Task<ResponseDTO> UpdateInfluByUserId(Guid userId, InfluUpdateDTO influDto);

    }
}
