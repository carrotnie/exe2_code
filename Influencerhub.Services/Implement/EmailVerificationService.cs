using Influencerhub.Common.DTO;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Influencerhub.Services.Implementation
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public EmailVerificationService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        private string GenerateEmailVerificationToken()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task SendVerificationLinkAsync(User user)
        {
            try
            {
                // 1. Sinh token và cập nhật user
                var token = GenerateEmailVerificationToken();
                user.EmailVerificationToken = token;
                user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(1);
                user.IsEmailVerified = false;
                await _userRepository.Update(user);

                // 2. Đọc cấu hình gửi mail
                var baseUrl = _configuration["BaseUrl"]; 
                var mailSettings = _configuration.GetSection("MailSettings");
                var fromEmail = mailSettings["FromEmail"];
                var fromName = mailSettings["FromName"];
                var fromPassword = mailSettings["AppPassword"];

                // Validate cấu hình
                if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(fromPassword))
                    throw new Exception("Cấu hình MailSettings bị thiếu (FromEmail hoặc AppPassword).");

                if (string.IsNullOrEmpty(baseUrl))
                    throw new Exception("Cấu hình BaseUrl bị thiếu trong appsettings.");

                // 3. Tạo link xác thực
                var verifyLink = $"{baseUrl}/api/Auth/verify-email?token={token}";

                // 4. Cấu hình SMTP client
                var fromAddress = new MailAddress(fromEmail, fromName ?? "InfluencerHub");
                var toAddress = new MailAddress(user.Email);
                const string subject = "Xác thực email InfluencerHub";
                string body = $"Chào bạn,<br>Vui lòng nhấn vào <a href='{verifyLink}'>đây</a> để xác thực email của bạn.<br>Nếu bạn không đăng ký, hãy bỏ qua email này.";

                using (var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
                })
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    Console.WriteLine($"[EmailVerification] Đang gửi email xác thực đến: {user.Email}");
                    await smtp.SendMailAsync(message);
                    Console.WriteLine($"[EmailVerification] Gửi mail thành công đến: {user.Email}");
                }
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết để kiểm tra lỗi khi gửi email
                Console.WriteLine("[EmailVerification][Lỗi gửi mail xác thực]: " + ex);
                throw;
            }
        }

        public async Task<ResponseDTO> VerifyTokenAsync(string token)
        {
            var user = await _userRepository.GetByEmailVerificationToken(token);
            if (user == null || user.EmailVerificationTokenExpiry < DateTime.UtcNow)
                return new ResponseDTO { IsSuccess = false, Message = "Link xác thực không hợp lệ hoặc đã hết hạn." };

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;
            await _userRepository.Update(user);

            return new ResponseDTO { IsSuccess = true, Message = "Xác thực email thành công!" };
        }
    }
}
