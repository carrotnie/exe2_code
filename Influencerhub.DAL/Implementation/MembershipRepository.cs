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
    }
}
