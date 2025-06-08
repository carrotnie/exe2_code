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

        public async Task<List<Business>> GetBusinessesByNameAsync(string name)
        {
            return await _context.Businesses
                .Where(b => b.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Business>> GetBusinessesByFieldNameAsync(string fieldName)
        {
            return await _context.Businesses
                .Where(b => _context.BusinessFields
                    .Any(bf => bf.BusinessId == b.Id && bf.Field != null && bf.Field.Name.ToLower().Contains(fieldName.ToLower())))
                .ToListAsync();
        }

        public async Task<List<Business>> GetBusinessesByAddressAsync(string address)
        {
            return await _context.Businesses
                .Where(b => b.Address != null && b.Address.ToLower().Contains(address.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Business>> GetAllBusinessesAsync()
        {
            return await _context.Businesses.ToListAsync();
        }

    }
}
