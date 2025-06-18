using System;
using System.Threading.Tasks;
using Influencerhub.Common.DTO;

namespace Influencerhub.Services.Contract
{
    public interface IUserService
    {
        Task<ResponseDTO> Login(UserDTO DTO);
        Task<ResponseDTO> Logout(Guid userId);
        Task<ResponseDTO> GenerateNewToken(string refreshToken);
        Task<ResponseDTO> ForgotPassword(ForgotPasswordRequest request);
        Task<ResponseDTO> ResetPassword(ResetPasswordRequest request);
        Task<ResponseDTO> UpdateUserVerificationStatus(Guid userId, bool isVerified);
        Task<ResponseDTO> UpdateUserBlockedStatus(Guid userId, bool isBlocked);

        Task<ResponseDTO> GetAllUsers();
        Task<ResponseDTO> GetUsersByVerificationStatus(bool isVerified);
        Task<ResponseDTO> GetUsersByEmailVerificationStatus(bool isEmailVerified);
        Task<ResponseDTO> GetBlockedUsers();

    }
}
