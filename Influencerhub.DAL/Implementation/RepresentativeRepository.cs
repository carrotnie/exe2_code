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
    public class RepresentativeRepository : IRepresentativeRepository
    {
        private readonly InfluencerhubDBContext _context;
        public RepresentativeRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<Representative> Add(Representative rep)
        {
            await _context.Representatives.AddAsync(rep);
            await _context.SaveChangesAsync();
            return rep;
        }

        public async Task<Representative?> GetByBusinessId(Guid businessId)
        {
            return await _context.Representatives.FirstOrDefaultAsync(r => r.BusinessId == businessId);
        }

        public async Task Update(Representative rep)
        {
            _context.Representatives.Update(rep);
            await _context.SaveChangesAsync();
        }

        public async Task<Representative?> GetRepresentativeByBusinessId(Guid businessId)
        {
            return await _context.Representatives
                .FirstOrDefaultAsync(r => r.BusinessId == businessId);
        }
    }

}
