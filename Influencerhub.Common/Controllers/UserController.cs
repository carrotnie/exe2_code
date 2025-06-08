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
        //[Authorize(Roles = "User,Admin")]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ResponseDTO> Login(UserDTO login)
        {
            return await _userService.Login(login);
        }


        [HttpPost("get-new-token")]
        public async Task<ResponseDTO> GenerateNewToken(string refreshToken)
        {
            return await _userService.GenerateNewToken(refreshToken);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _userService.ForgotPassword(request);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _userService.ResetPassword(request);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var result = await _userService.Logout(request.UserId);
            return Ok(result);
        }

    }
}
