using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Implementation
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly InfluencerhubDBContext _context;

        public BusinessRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<Business> CreateBusiness(Business business)
        {
            await _context.Businesses.AddAsync(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<Business> UpdateBusiness(Business business)
        {
            _context.Businesses.Update(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<Business?> GetBusinessByIdAsync(Guid businessId)
        {
            return await _context.Businesses.FindAsync(businessId);
        }
    }
}
