using Influencerhub.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Contract
{
    public interface IMembershipTypeRepository
    {
        Task<List<MembershipType>> GetAll();
        Task<List<MembershipType>> GetByBusiness();
        Task<List<MembershipType>> GetByKol();
        Task<MembershipType?> GetById(Guid id);
        Task<MembershipType> Add(MembershipType entity);
        Task<MembershipType> Update(MembershipType entity);
        Task<bool> Delete(Guid id);
    }

}
