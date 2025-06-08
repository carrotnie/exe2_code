using Influencerhub.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IMembershipTypeService
    {
        Task<List<MembershipTypeDTO>> GetAll();
        Task<List<MembershipTypeDTO>> GetByBusiness();
        Task<List<MembershipTypeDTO>> GetByKol();
        Task<MembershipTypeDTO?> GetById(Guid id);
        Task<MembershipTypeDTO> Add(MembershipTypeDTO dto);
        Task<MembershipTypeDTO> Update(MembershipTypeDTO dto);
        Task<bool> Delete(Guid id);
    }

}
