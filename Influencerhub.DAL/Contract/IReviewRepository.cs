using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Influencerhub.DAL.Models;

namespace Influencerhub.DAL.Contract
{
    public interface IReviewRepository
    {
        Task<Review> AddAsync(Review review);
        Task<List<Review>> GetReviewsByInfluIdAsync(Guid influId);
        Task<List<Review>> GetReviewsByBusinessIdAsync(Guid businessId);
    }
}
