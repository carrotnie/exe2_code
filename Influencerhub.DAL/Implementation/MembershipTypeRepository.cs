using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Implementation
{
    public class MembershipTypeRepository : IMembershipTypeRepository
    {
        private readonly InfluencerhubDBContext _context;

        public MembershipTypeRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<List<MembershipType>> GetAll()
        {
            return await _context.MembershipTypes.ToListAsync();
        }

        public async Task<List<MembershipType>> GetByBusiness()
        {
            return await _context.MembershipTypes
                .Where(x => x.Name.Contains("Business"))
                .ToListAsync();
        }

        public async Task<List<MembershipType>> GetByKol()
        {
            return await _context.MembershipTypes
                .Where(x => x.Name.Contains("KOL"))
                .ToListAsync();
        }

        public async Task<MembershipType?> GetById(Guid id)
        {
            return await _context.MembershipTypes.FindAsync(id);
        }

        public async Task<MembershipType> Add(MembershipType entity)
        {
            _context.MembershipTypes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<MembershipType> Update(MembershipType entity)
        {
            _context.MembershipTypes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _context.MembershipTypes.FindAsync(id);
            if (entity == null) return false;
            _context.MembershipTypes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
