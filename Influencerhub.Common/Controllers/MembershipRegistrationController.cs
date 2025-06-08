using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Influencerhub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipRegistrationController : ControllerBase
    {
        private readonly IMembershipRegistrationService _service;

        public MembershipRegistrationController(IMembershipRegistrationService service)
        {
            _service = service;
        }

        [HttpPost("register-membership")]
        public async Task<IActionResult> RegisterMembership([FromBody] RegisterMembershipRequest request)
        {
            try
            {
                var result = await _service.RegisterMembershipAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}
