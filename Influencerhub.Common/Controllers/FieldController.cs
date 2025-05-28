using Microsoft.AspNetCore.Mvc;
using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Influencerhub.API.Controllers
{
    [Route("api/field")]
    [ApiController]
    public class FieldController : ControllerBase
    {
        private readonly IFieldService _fieldService;

        public FieldController(IFieldService fieldService)
        {
            _fieldService = fieldService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ResponseDTO> CreateField([FromBody] FieldDTO fieldDto)
        {
            return await _fieldService.CreateFieldAsync(fieldDto);
        }

        [HttpPut("update/{id}")]
        public async Task<ResponseDTO> UpdateFieldById([FromRoute] Guid id, [FromBody] FieldDTO fieldDto)
        {
            return await _fieldService.UpdateFieldAsync(id, fieldDto.Name);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ResponseDTO> DeleteFieldById([FromRoute] Guid id)
        {
            return await _fieldService.DeleteFieldAsync(id);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ResponseDTO> GetFieldById([FromRoute] Guid id)
        {
            return await _fieldService.GetFieldByIdAsync(id);
        }

        [HttpGet("get-by-name/{name}")]
        public async Task<ResponseDTO> GetFieldsByName([FromRoute] string name)
        {
            return await _fieldService.GetFieldsByNameAsync(name);
        }

        [HttpGet("get-all")]
        public async Task<ResponseDTO> GetAllFields()
        {
            return await _fieldService.GetAllFieldsAsync();
        }
    }
}
