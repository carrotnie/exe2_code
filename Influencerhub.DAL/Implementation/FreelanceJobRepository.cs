using Influencerhub.DAL.Models;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Influencerhub.Common.Enum;

namespace Influencerhub.DAL.Implementation
{
    public class FreelanceJobRepository : IFreelanceJobRepository
    {
        private readonly InfluencerhubDBContext _context;

        public FreelanceJobRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<FreelanceJob?> GetByJobAndFreelance(Guid jobId, Guid freelanceId)
        {
            return await _context.FreelanceJobs
                .FirstOrDefaultAsync(x => x.JobId == jobId && x.FreelanceId == freelanceId);
        }

        public async Task<FreelanceJob> ApplyForJob(Guid jobId, Guid freelanceId)
        {
            var entity = new FreelanceJob
            {
                Id = Guid.NewGuid(),
                JobId = jobId,
                FreelanceId = freelanceId,
                status = FreelanceJobStatus.NotYetConfirmed,
                StartTime = null,
                EndTime = null,
                CancelTime = null
            };

            _context.FreelanceJobs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<FreelanceJob> Add(FreelanceJob entity)
        {
            _context.FreelanceJobs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<FreelanceJob>> GetByJobId(Guid jobId)
        {
            return await _context.FreelanceJobs
                .Where(x => x.JobId == jobId)
                .Include(x => x.Influ)
                .ToListAsync();
        }
        public async Task<FreelanceJob?> GetById(Guid id)
        {
            return await _context.FreelanceJobs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<FreelanceJob> Update(FreelanceJob entity)
        {
            _context.FreelanceJobs.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<FreelanceJob>> GetByInfluIdAndStatus(Guid influId, FreelanceJobStatus status)
        {
            return await _context.FreelanceJobs
                .Where(x => x.FreelanceId == influId && x.status == status)
                .Include(x => x.Job)
                .ToListAsync();
        }


    }
}
