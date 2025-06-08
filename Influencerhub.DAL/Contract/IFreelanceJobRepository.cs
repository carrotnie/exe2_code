using Influencerhub.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Contract
{
    public interface IFreelanceJobRepository
    {
        Task<FreelanceJob> ApplyForJob(Guid jobId, Guid freelanceId);
        Task<FreelanceJob?> GetByJobAndFreelance(Guid jobId, Guid freelanceId);
        Task<FreelanceJob> Add(FreelanceJob entity);
        Task<List<FreelanceJob>> GetByJobId(Guid jobId);
        Task<FreelanceJob?> GetById(Guid id);
        Task<FreelanceJob> Update(FreelanceJob entity); 
    }
}
