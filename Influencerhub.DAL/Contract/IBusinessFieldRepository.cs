using Influencerhub.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Contract
{
    public interface IBusinessFieldRepository
    {
        Task AddRange(IEnumerable<BusinessField> businessFields);
        Task RemoveRangeByBusinessId(Guid businessId);
    }
}
