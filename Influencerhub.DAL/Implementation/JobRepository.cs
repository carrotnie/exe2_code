using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;
using Microsoft.EntityFrameworkCore;

public class JobRepository : IJobRepository
{
    private readonly InfluencerhubDBContext _context;
    public JobRepository(InfluencerhubDBContext context)
    {
        _context = context;
    }

    public async Task<List<Job>> GetAll()
    {
        return await _context.Jobs.Include(j => j.Business).Include(j => j.BusinessField).ToListAsync();
    }

    public async Task<Job> Add(Job job)
    {
        await _context.Jobs.AddAsync(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<Job> Update(Guid id, UpdateJobDTO jobDto)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) throw new Exception("Job not found");

        // Chỉ cho phép update khi Status là Available
        if (job.Status != JobStatus.Available)
            throw new Exception("Chỉ được cập nhật khi chưa có ai đăng ký job");

        job.Title = jobDto.Title;
        job.Description = jobDto.Description;
        job.Location = jobDto.Location;
        job.Budget = jobDto.Budget;
        job.StartTime = jobDto.StartTime;
        job.EndTime = jobDto.EndTime;
        job.Require = jobDto.Require;
        job.KolBenefits = jobDto.KolBenefits;
        job.Gender = jobDto.Gender;
        job.Follower = jobDto.Follower;

        await _context.SaveChangesAsync();
        return job;
    }



    public async Task<bool> Delete(Guid id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return false;
        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Job> GetJobById(Guid id)
    {
        return await _context.Jobs
            .Include(j => j.Business)
            .Include(j => j.BusinessField)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<List<Job>> GetJobsByBusinessName(string businessName)
    {
        return await _context.Jobs
            .Include(j => j.Business)
            .Where(j => j.Business != null && j.Business.Name.Contains(businessName))
            .ToListAsync();
    }

    public async Task<List<Job>> GetJobsByBusinessField(Guid businessFieldId)
    {
        return await _context.Jobs
            .Where(j => j.BusinessFieldId == businessFieldId)
            .ToListAsync();
    }

    public async Task<List<Job>> GetJobsByFieldName(string fieldName)
    {
        // Tìm tất cả Job có BusinessField liên kết với Field có tên phù hợp
        return await _context.Jobs
            .Include(j => j.Business)
            .Include(j => j.BusinessField)
                .ThenInclude(bf => bf.Field)
            .Where(j => j.BusinessField != null
                        && j.BusinessField.Field != null
                        && j.BusinessField.Field.Name.Contains(fieldName))
            .ToListAsync();
    }

    public async Task<List<Job>> FilterJobsByLocation(string location)
    {
        return await _context.Jobs
            .Where(j => j.Location != null && j.Location.Contains(location))
            .ToListAsync();
    }

    public async Task<List<Job>> FilterJobsByBudget(decimal minBudget, decimal maxBudget)
    {
        return await _context.Jobs
            .Where(j => j.Budget >= minBudget && j.Budget <= maxBudget)
            .ToListAsync();
    }

    public async Task<List<Job>> FilterJobsByStartDate(DateTime fromDate)
    {
        return await _context.Jobs
            .Where(j => j.StartTime != null && j.StartTime.Value.Date >= fromDate.Date)
            .ToListAsync();
    }


    public async Task<bool> UpdateJobStatus(Guid id, JobStatus newStatus)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return false;
        job.Status = newStatus;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Job>> GetJobsByBusinessId(Guid businessId)
    {
        return await _context.Jobs
            .Include(j => j.Business)
            .Include(j => j.BusinessField)
            .Where(j => j.BusinessId == businessId)
            .ToListAsync();
    }

    public async Task<List<Job>> GetJobsByStatus(JobStatus status)
    {
        return await _context.Jobs
            .Where(j => j.Status == status)
            .Include(j => j.Business)
            .Include(j => j.BusinessField)
            .ToListAsync();
    }

    public async Task<List<Job>> GetJobsByStatusAndBusinessId(JobStatus status, Guid businessId)
    {
        return await _context.Jobs
            .Where(j => j.Status == status && j.BusinessId == businessId)
            .Include(j => j.Business)
            .Include(j => j.BusinessField)
            .ToListAsync();
    }

}
