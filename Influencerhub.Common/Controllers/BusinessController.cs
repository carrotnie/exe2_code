using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Influencerhub.API.Controllers
{
    [Route("api/business")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ResponseDTO> CreateBusiness([FromBody] BusinessDTO dto)
        {
            return await _businessService.CreateBusiness(dto);
        }

        [HttpPut("update-by-user/{userId}")]
        public async Task<ResponseDTO> UpdateBusinessByUserId([FromRoute] Guid userId, [FromBody] BusinessUpdateDTO dto)
        {
            return await _businessService.UpdateBusinessByUserId(userId, dto);
        }

    }
}
