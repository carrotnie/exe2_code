using Microsoft.AspNetCore.Mvc;
using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Influencerhub.API.Controllers
{
    [Route("api/influ")]
    [ApiController]
    public class InfluController : ControllerBase
    {
        private readonly IInfluService _influService;

        public InfluController(IInfluService influService)
        {
            _influService = influService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ResponseDTO> CreateInflu([FromBody] InfluDTO influ)
        {
            return await _influService.CreateInflu(influ);
        }

        [HttpPut("update-by-user/{userId}")]
        public async Task<ResponseDTO> UpdateInfluByUserId([FromRoute] Guid userId, [FromBody] InfluUpdateDTO influ)
        {
            return await _influService.UpdateInfluByUserId(userId, influ);
        }

    }
}
