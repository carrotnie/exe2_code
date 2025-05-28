using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using Influencerhub.Common.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Influencerhub.Services.Implementation
{
    public class FieldService : IFieldService
    {
        private readonly IFieldRepository _fieldRepository;

        public FieldService(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }

        public async Task<ResponseDTO> CreateFieldAsync(FieldDTO dto)
        {
            var response = new ResponseDTO();
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new Exception("Tên lĩnh vực không được để trống!");

                var field = new Field { Name = dto.Name };
                var result = await _fieldRepository.CreateAsync(field);

                response.IsSuccess = true;
                response.Data = result;
                response.Message = "Tạo lĩnh vực thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Tạo lĩnh vực thất bại: " + ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> DeleteFieldAsync(Guid id)
        {
            var response = new ResponseDTO();
            try
            {
                var success = await _fieldRepository.DeleteAsync(id);
                if (!success)
                    throw new Exception("Không tìm thấy lĩnh vực để xóa!");

                response.IsSuccess = true;
                response.Message = "Xóa lĩnh vực thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Xóa lĩnh vực thất bại: " + ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> UpdateFieldAsync(Guid id, string name)
        {
            var response = new ResponseDTO();
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new Exception("Tên lĩnh vực không được để trống!");

                var field = await _fieldRepository.GetByIdAsync(id);
                if (field == null)
                    throw new Exception("Không tìm thấy lĩnh vực!");

                field.Name = name;
                var updated = await _fieldRepository.UpdateAsync(field);

                response.IsSuccess = true;
                response.Data = updated;
                response.Message = "Cập nhật lĩnh vực thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Cập nhật lĩnh vực thất bại: " + ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> GetFieldByIdAsync(Guid id)
        {
            var response = new ResponseDTO();
            try
            {
                var field = await _fieldRepository.GetByIdAsync(id);
                if (field == null)
                    throw new Exception("Không tìm thấy lĩnh vực!");

                response.IsSuccess = true;
                response.Data = field;
                response.Message = "Lấy lĩnh vực thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> GetFieldsByNameAsync(string name)
        {
            var response = new ResponseDTO();
            try
            {
                var fields = await _fieldRepository.GetByNameContainsAsync(name);
                response.IsSuccess = true;
                response.Data = fields;
                response.Message = fields.Count == 0
                    ? "Không tìm thấy lĩnh vực phù hợp!"
                    : $"Đã tìm thấy {fields.Count} lĩnh vực phù hợp!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }


        public async Task<ResponseDTO> GetAllFieldsAsync()
        {
            var response = new ResponseDTO();
            try
            {
                var fields = await _fieldRepository.GetAllAsync();
                response.IsSuccess = true;
                response.Data = fields;
                response.Message = "Lấy tất cả lĩnh vực thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Lấy dữ liệu thất bại: " + ex.Message;
            }
            return response;
        }
    }
}
