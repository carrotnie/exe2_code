using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Influencerhub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipTypeController : ControllerBase
    {
        private readonly IMembershipTypeService _service;

        public MembershipTypeController(IMembershipTypeService service)
        {
            _service = service;
        }

        [HttpGet("Get-all-membershiptype")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(data);
        }

        [HttpGet("Get-all-membershiptype-business")]
        public async Task<IActionResult> GetBusiness()
        {
            var data = await _service.GetByBusiness();
            return Ok(data);
        }

        [HttpGet("Get-all-membershiptype-kol")]
        public async Task<IActionResult> GetKol()
        {
            var data = await _service.GetByKol();
            return Ok(data);
        }

    }

}
