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
    public class BusinessFieldRepository : IBusinessFieldRepository
    {
        private readonly InfluencerhubDBContext _context;
        public BusinessFieldRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task AddRange(IEnumerable<BusinessField> businessFields)
        {
            await _context.BusinessFields.AddRangeAsync(businessFields);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeByBusinessId(Guid businessId)
        {
            var fields = _context.BusinessFields.Where(bf => bf.BusinessId == businessId);
            _context.BusinessFields.RemoveRange(fields);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BusinessField>> GetByBusinessIdAsync(Guid businessId)
        {
            return await _context.BusinessFields
                .Where(bf => bf.BusinessId == businessId)
                .ToListAsync();
        }
    }

}
