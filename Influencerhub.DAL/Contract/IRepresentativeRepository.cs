using Influencerhub.DAL.Models;
using System;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Contract
{
    public interface IRepresentativeRepository
    {
        Task<Representative> Add(Representative rep);
        Task<Representative?> GetByBusinessId(Guid businessId);
        Task Update(Representative rep);
    }
}
