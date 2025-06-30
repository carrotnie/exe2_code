using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Influencerhub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IConfiguration _configuration;

        public AuthController(IEmailVerificationService emailVerificationService, IConfiguration configuration)
        {
            _emailVerificationService = emailVerificationService;
            _configuration = configuration;
        }

        // Endpoint xác thực khi người dùng nhấn link
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var response = await _emailVerificationService.VerifyTokenAsync(token);

            var successUrl = _configuration["SuccessUrl"];
            var failedUrl = _configuration["FailedUrl"];

            if (response.IsSuccess)
            {
                // Redirect về trang báo xác thực thành công
                return Redirect(successUrl); // -> https://influencerhub.id.vn/success
            }
            // Redirect về trang báo thất bại
            return Redirect(failedUrl); // -> https://influencerhub.id.vn/email-verify-failed
        }
    }
}
