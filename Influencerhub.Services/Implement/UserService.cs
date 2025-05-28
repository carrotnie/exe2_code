using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Influencerhub.Common.DTO;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Implementation;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;

namespace Influencerhub.Services.Implementation
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IConfiguration _configuration;
        private IJWTService _jwtService;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IJWTService jwtService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _jwtService = jwtService;
        }



        public async Task<ResponseDTO> GenerateNewToken(string refreshToken)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var user = await _userRepository.GetUserByRefreshToken(refreshToken);
                if (user == null)
                {
                    responseDTO.BusinessCode = Common.Enum.BusinessCode.NotFound;
                    responseDTO.Message = "Không tìm thấy refreshToken trong csdl";
                    return responseDTO;
                }
                if (user.ExpireTimeRefreshToken < DateTime.Now)
                {
                    responseDTO.BusinessCode = Common.Enum.BusinessCode.ExpireToken;
                    responseDTO.Message = "RefreshToken hết hạn vui lòng login lại";
                    return responseDTO;
                }
                user.RefreshToken = string.Empty;
                user.ExpireTimeRefreshToken = DateTime.Now;
                await _userRepository.Update(user);

                responseDTO.Data = new TokenResponse
                {
                    AccessToken = _jwtService.GenerateAccessToken(user),
                    RefreshToken = string.Empty
                };
            }
            catch (Exception ex)
            {
                responseDTO.BusinessCode = Influencerhub.Common.Enum.BusinessCode.UnknownServerError;
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Login(UserDTO DTO)
        {
            var responseDTO = new ResponseDTO();
            try
            {
                // Lấy user theo email/password, PHẢI kèm Role (Repository đã Include Role)
                var userDb = await _userRepository.Login(DTO.Email, DTO.Password);

                if (userDb != null)
                {
                    // Kiểm tra role tồn tại chưa
                    if (userDb.Role == null || string.IsNullOrEmpty(userDb.Role.Name))
                    {
                        responseDTO.BusinessCode = Influencerhub.Common.Enum.BusinessCode.NotFound;
                        responseDTO.IsSuccess = false;
                        responseDTO.Message = "Tài khoản chưa được gán quyền hoặc dữ liệu quyền chưa hợp lệ!";
                        return responseDTO;
                    }

                    // Sinh refresh token và cập nhật thời gian hết hạn
                    userDb.RefreshToken = _jwtService.GenerateRefreshToken();
                    userDb.ExpireTimeRefreshToken = DateTime.Now.AddDays(
                        double.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"]));
                    await _userRepository.Update(userDb);

                    // Sinh access token (đảm bảo role đã có giá trị)
                    responseDTO.Data = new TokenResponse
                    {
                        AccessToken = _jwtService.GenerateAccessToken(userDb),
                        RefreshToken = userDb.RefreshToken
                    };
                    responseDTO.IsSuccess = true;
                    responseDTO.Message = "Đăng nhập thành công";
                }
                else
                {
                    responseDTO.BusinessCode = Influencerhub.Common.Enum.BusinessCode.NotFound;
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Email hoặc mật khẩu không đúng";
                }
            }
            catch (Exception ex)
            {
                responseDTO.BusinessCode = Influencerhub.Common.Enum.BusinessCode.UnknownServerError;
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }





    }
}
