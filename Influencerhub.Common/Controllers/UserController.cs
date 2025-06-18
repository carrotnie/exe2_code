using Microsoft.AspNetCore.Mvc;
using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Influencerhub.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ResponseDTO> Login(UserDTO login)
        {
            return await _userService.Login(login);
        }

        [AllowAnonymous]
        [HttpPost("get-new-token")]
        public async Task<ResponseDTO> GenerateNewToken(string refreshToken)
        {
            return await _userService.GenerateNewToken(refreshToken);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _userService.ForgotPassword(request);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _userService.ResetPassword(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var result = await _userService.Logout(request.UserId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("unverified")]
        public async Task<IActionResult> GetUnverifiedUsers()
        {
            var result = await _userService.GetUsersByVerificationStatus(false);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("verified")]
        public async Task<IActionResult> GetVerifiedUsers()
        {
            var result = await _userService.GetUsersByVerificationStatus(true);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("email-unverified")]
        public async Task<IActionResult> GetEmailUnverifiedUsers()
        {
            var result = await _userService.GetUsersByEmailVerificationStatus(false);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("email-verified")]
        public async Task<IActionResult> GetEmailVerifiedUsers()
        {
            var result = await _userService.GetUsersByEmailVerificationStatus(true);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("blocked")]
        public async Task<IActionResult> GetBlockedUsers()
        {
            var result = await _userService.GetBlockedUsers();
            return Ok(result);
        }

    }
}
