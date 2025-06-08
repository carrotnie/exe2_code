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
using BCrypt.Net;

namespace Influencerhub.Services.Implementation
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IConfiguration _configuration;
        private IJWTService _jwtService;
        private IEmailService _emailService;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IJWTService jwtService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _jwtService = jwtService;
            _emailService = emailService;
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
                var userDb = await _userRepository.Login(DTO.Email, DTO.Password);

                if (userDb != null)
                {
                    // *** KIỂM TRA ĐỦ 2 ĐIỀU KIỆN ***
                    if (!userDb.IsVerified)
                    {
                        responseDTO.IsSuccess = false;
                        responseDTO.Message = "Tài khoản của bạn chưa được admin phê duyệt!";
                        return responseDTO;
                    }
                    if (!userDb.IsEmailVerified)
                    {
                        responseDTO.IsSuccess = false;
                        responseDTO.Message = "Bạn chưa xác thực email!";
                        return responseDTO;
                    }
                    if (userDb.IsBlocked)
                    {
                        responseDTO.IsSuccess = false;
                        responseDTO.Message = "Tài khoản của bạn đã bị chặn.";
                        return responseDTO;
                    }


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


        public async Task<ResponseDTO> ForgotPassword(ForgotPasswordRequest request)
        {
            var response = new ResponseDTO();
            var user = await _userRepository.GetByEmail(request.Email);
            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Email này chưa được đăng ký trên hệ thống.";
                return response;
            }

            // Tạo token
            string token = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(); // Hoặc random 6-8 ký tự cho dễ nhập
            user.ResetPasswordToken = token;
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddMinutes(10); // Token hết hạn trong 10 phút
            await _userRepository.Update(user);

            // Gửi token qua mail (không gửi link)
            await _emailService.SendPasswordResetTokenAsync(user.Email, token);

            response.IsSuccess = true;
            response.Message = "Mã xác thực đặt lại mật khẩu đã được gửi đến email của bạn.";
            return response;
        }



        public async Task<ResponseDTO> ResetPassword(ResetPasswordRequest request)
        {
            var response = new ResponseDTO();
            var user = await _userRepository.GetByResetPasswordToken(request.Token);
            if (user == null || user.ResetPasswordTokenExpiry < DateTime.UtcNow)
            {
                response.IsSuccess = false;
                response.Message = "Token không hợp lệ hoặc đã hết hạn!";
                return response;
            }
            // Không hash, lưu plain text
            user.Password = request.NewPassword;
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;
            await _userRepository.Update(user);

            response.IsSuccess = true;
            response.Message = "Đổi mật khẩu thành công!";
            return response;
        }

        public async Task<ResponseDTO> Logout(Guid userId)
        {
            var response = new ResponseDTO();
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy người dùng.";
                return response;
            }

            // Xoá refresh token và hạn sử dụng
            user.RefreshToken = string.Empty;
            user.ExpireTimeRefreshToken = DateTime.Now;
            await _userRepository.Update(user);

            response.IsSuccess = true;
            response.Message = "Đăng xuất thành công!";
            return response;
        }

        public async Task<ResponseDTO> UpdateUserVerificationStatus(Guid userId, bool isVerified)
        {
            var response = new ResponseDTO();
            try
            {
                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Không tìm thấy người dùng.";
                    return response;
                }

                user.IsVerified = isVerified;
                await _userRepository.Update(user);

                response.IsSuccess = true;
                response.Message = $"Đã {(isVerified ? "duyệt" : "hủy duyệt")} tài khoản thành công.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> UpdateUserBlockedStatus(Guid userId, bool isBlocked)
        {
            var response = new ResponseDTO();
            try
            {
                var user = await _userRepository.UpdateBlockedStatus(userId, isBlocked);
                response.IsSuccess = true;
                response.Message = $"Đã {(isBlocked ? "chặn" : "mở chặn")} tài khoản thành công.";
                response.Data = user;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }


    }
}
