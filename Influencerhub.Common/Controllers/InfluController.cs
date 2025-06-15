using Microsoft.AspNetCore.Mvc;
using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Influencerhub.Services.Implementation;

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

        [Authorize(Roles = "Freelancer")]
        [HttpPut("update-by-user/{userId}")]
        public async Task<ResponseDTO> UpdateInfluByUserId([FromRoute] Guid userId, [FromBody] InfluUpdateDTO influ)
        {
            return await _influService.UpdateInfluByUserId(userId, influ);
        }

        [AllowAnonymous]
        [HttpGet("search-by-name")]
        public async Task<ResponseDTO> SearchByName([FromQuery] string keyword)
        {
            return await _influService.SearchByName(keyword);
        }

        [AllowAnonymous]
        [HttpGet("search-by-field")]
        public async Task<ResponseDTO> SearchByFieldName([FromQuery] string keyword)
        {
            return await _influService.SearchByFieldName(keyword);
        }

        [AllowAnonymous]
        [HttpGet("search-by-area")]
        public async Task<ResponseDTO> SearchByArea([FromQuery] string keyword)
        {
            return await _influService.SearchByArea(keyword);
        }

        [AllowAnonymous]
        [HttpGet("search-by-follower")]
        public async Task<ResponseDTO> SearchByFollower([FromQuery] int? minFollower, [FromQuery] int? maxFollower)
        {
            return await _influService.SearchByFollower(minFollower, maxFollower);
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ResponseDTO> GetAll()
        {
            return await _influService.GetAllInflu();
        }

        [AllowAnonymous]
        [HttpGet("get-influ-by-id/{influId}")]
        public async Task<ResponseDTO> GetById([FromRoute] Guid influId)
        {
            return await _influService.GetById(influId);
        }

        [AllowAnonymous]
        [HttpGet("get-influ-by-userId/{userId}")]
        public async Task<ResponseDTO> GetByUserId([FromRoute] Guid userId)
        {
            return await _influService.GetByUserId(userId);
        }

    }
}
