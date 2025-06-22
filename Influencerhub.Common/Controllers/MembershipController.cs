using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Influencerhub.API.Controllers
{
    [Route("api/membership")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await _membershipService.GetByUserId(userId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _membershipService.GetAll();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("influencers")]
        public async Task<IActionResult> GetInfluencerMemberships()
        {
            var result = await _membershipService.GetInfluencerMemberships();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("businesses")]
        public async Task<IActionResult> GetBusinessMemberships()
        {
            var result = await _membershipService.GetBusinessMemberships();
            return Ok(result);
        }
    }
}
