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


        [Authorize(Roles = "Business")]
        [HttpPut("update-by-user/{userId}")]
        public async Task<ResponseDTO> UpdateBusinessByUserId([FromRoute] Guid userId, [FromBody] BusinessUpdateDTO dto)
        {
            return await _businessService.UpdateBusinessByUserId(userId, dto);

        }

        [AllowAnonymous]
        [HttpGet("search-by-name")]
        public async Task<ResponseDTO> SearchByName([FromQuery] string name)
        {
            return await _businessService.SearchBusinessByName(name);
        }

        [AllowAnonymous]
        [HttpGet("search-by-field")]
        public async Task<ResponseDTO> SearchByField([FromQuery] string fieldName)
        {
            return await _businessService.SearchBusinessByField(fieldName);
        }

        [AllowAnonymous]
        [HttpGet("search-by-address")]
        public async Task<ResponseDTO> SearchByAddress([FromQuery] string address)
        {
            return await _businessService.SearchBusinessByAddress(address);
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ResponseDTO> GetAll()
        {
            return await _businessService.GetAllBusinesses();
        }

    }
}
