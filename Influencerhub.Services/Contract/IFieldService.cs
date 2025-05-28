using Influencerhub.Common.DTO;
using System;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IFieldService
    {
        Task<ResponseDTO> CreateFieldAsync(FieldDTO dto);
        Task<ResponseDTO> DeleteFieldAsync(Guid id);
        Task<ResponseDTO> UpdateFieldAsync(Guid id, string name);
        Task<ResponseDTO> GetFieldByIdAsync(Guid id);
        Task<ResponseDTO> GetFieldsByNameAsync(string name); // trả về nhiều kết quả
        Task<ResponseDTO> GetAllFieldsAsync();
    }
}
