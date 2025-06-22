using Influencerhub.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Contract
{
    public interface IMembershipRepository
    {
        Task<Membership?> GetByUserId(Guid userId);
        Task<List<Membership>> GetAll();
        Task<List<Membership>> GetInfluencerMemberships();
        Task<List<Membership>> GetBusinessMemberships();
    }
}
