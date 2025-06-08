using Microsoft.AspNetCore.Mvc;
using Influencerhub.Services.Contract;
using System;
using System.Threading.Tasks;

namespace Influencerhub.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("users/{userId}/verify-account")]
        public async Task<IActionResult> UpdateUserVerificationStatus(Guid userId, [FromQuery] bool isVerified)
        {
            var result = await _userService.UpdateUserVerificationStatus(userId, isVerified);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("users/{userId}/block")]
        public async Task<IActionResult> UpdateUserBlockedStatus(Guid userId, [FromQuery] bool isBlocked)
        {
            var result = await _userService.UpdateUserBlockedStatus(userId, isBlocked);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
