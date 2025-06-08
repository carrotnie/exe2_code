using Influencerhub.Common.DTO;
using Influencerhub.DAL.Models;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IEmailVerificationService
    {
        Task SendVerificationLinkAsync(User user);
        Task<ResponseDTO> VerifyTokenAsync(string token);
    }
}
