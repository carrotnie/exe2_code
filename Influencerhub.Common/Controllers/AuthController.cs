using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Influencerhub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEmailVerificationService _emailVerificationService;

        public AuthController(IEmailVerificationService emailVerificationService)
        {
            _emailVerificationService = emailVerificationService;
        }

        // Endpoint xác thực khi người dùng nhấn link
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var response = await _emailVerificationService.VerifyTokenAsync(token);
            if (response.IsSuccess)
            {
                // Redirect về trang báo xác thực thành công (có thể là FE SPA, hoặc 1 trang tĩnh)
                return Redirect("https://influencerhub.id.vn/success"); // FE tự làm trang này
            }
            // Redirect về trang báo thất bại
            return Redirect("https://yourdomain.com/email-verify-failed");
        }
    }
}
