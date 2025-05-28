using Influencerhub.DAL.Models;
using System;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Contract
{
    public interface IBusinessRepository
    {
        Task<Business> CreateBusiness(Business business);
        Task<Business> UpdateBusiness(Business business);
        Task<Business?> GetBusinessByIdAsync(Guid businessId);
    }
}
