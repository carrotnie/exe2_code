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
        Task<List<Business>> GetBusinessesByNameAsync(string name);
        Task<List<Business>> GetBusinessesByFieldNameAsync(string fieldName);
        Task<List<Business>> GetBusinessesByAddressAsync(string address);
        Task<List<Business>> GetAllBusinessesAsync();

    }
}
