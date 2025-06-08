using Influencerhub.Common.DTO;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Services.Implement
{
    public class MembershipTypeService : IMembershipTypeService
    {
        private readonly IMembershipTypeRepository _repo;

        public MembershipTypeService(IMembershipTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<MembershipTypeDTO>> GetAll()
        {
            var data = await _repo.GetAll();
            return data.Select(x => ToDTO(x)).ToList();
        }

        public async Task<List<MembershipTypeDTO>> GetByBusiness()
        {
            var data = await _repo.GetByBusiness();
            return data.Select(x => ToDTO(x)).ToList();
        }

        public async Task<List<MembershipTypeDTO>> GetByKol()
        {
            var data = await _repo.GetByKol();
            return data.Select(x => ToDTO(x)).ToList();
        }

        public async Task<MembershipTypeDTO?> GetById(Guid id)
        {
            var item = await _repo.GetById(id);
            return item != null ? ToDTO(item) : null;
        }

        public async Task<MembershipTypeDTO> Add(MembershipTypeDTO dto)
        {
            var entity = new MembershipType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                Price = dto.Price,
                Description = dto.Description
            };
            var added = await _repo.Add(entity);
            return ToDTO(added);
        }

        public async Task<MembershipTypeDTO> Update(MembershipTypeDTO dto)
        {
            var entity = await _repo.GetById(dto.Id);
            if (entity == null) throw new Exception("Not found");

            entity.Name = dto.Name;
            entity.Type = dto.Type;
            entity.Price = dto.Price;
            entity.Description = dto.Description;

            var updated = await _repo.Update(entity);
            return ToDTO(updated);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repo.Delete(id);
        }

        private MembershipTypeDTO ToDTO(MembershipType x) =>
            new MembershipTypeDTO
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Price = x.Price,
                Description = x.Description
            };
    }

}
