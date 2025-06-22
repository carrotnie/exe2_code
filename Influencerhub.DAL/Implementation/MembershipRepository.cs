using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Implementation
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly InfluencerhubDBContext _context;

        public MembershipRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<Membership?> GetByUserId(Guid userId)
        {
            return await _context.Memberships
                .Include(m => m.User)
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<List<Membership>> GetAll()
        {
            return await _context.Memberships
                .Include(m => m.User)
                .Include(m => m.MembershipType)
                .ToListAsync();
        }

        public async Task<List<Membership>> GetInfluencerMemberships()
        {
            var freelancerRoleId = Guid.Parse("BFBA4EA3-E5FC-4716-B1CF-8DD44BFD23B8");

            return await _context.Memberships
                .Include(m => m.User)
                .Include(m => m.MembershipType)
                .Where(m => m.User != null && m.User.RoleId == freelancerRoleId)
                .ToListAsync();
        }


        public async Task<List<Membership>> GetBusinessMemberships()
        {
            var businessRoleId = Guid.Parse("5A1688AD-222D-498E-B3FF-C61B3D5476DA");

            return await _context.Memberships
                .Include(m => m.User)
                .Include(m => m.MembershipType)
                .Where(m => m.User != null && m.User.RoleId == businessRoleId)
                .ToListAsync();
        }

    }
}
