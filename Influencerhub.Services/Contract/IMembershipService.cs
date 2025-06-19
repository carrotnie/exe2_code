using Influencerhub.Common.DTO;
using System;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IMembershipService
    {
        Task<ResponseDTO> GetByUserId(Guid userId);
        Task<ResponseDTO> GetAll();
    }
}
