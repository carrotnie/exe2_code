using Influencerhub.Common.Enum;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly InfluencerhubDBContext _context;

        public ReviewRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<Review> AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<List<Review>> GetReviewsByInfluIdAsync(Guid influId)
        {
            return await _context.Reviews
                .Where(r => r.InfluId == influId && r.Type == ReviewType.BusinessToInflu)
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByBusinessIdAsync(Guid businessId)
        {
            return await _context.Reviews
                .Where(r => r.BusinessId == businessId && r.Type == ReviewType.InfluToBusiness)
                .ToListAsync();
        }
    }
}
