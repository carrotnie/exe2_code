using Influencerhub.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Contract
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction entity);
        Task<MembershipType?> GetMembershipTypeAsync(Guid membershipTypeId);
        Task<User?> GetUserAsync(Guid userId);
    }

}
