using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Models;
public interface IJobRepository
{
    Task<List<Job>> GetAll();
    Task<Job> Add(Job job);
    Task<Job> Update(Guid id, UpdateJobDTO jobDto);
    Task<bool> Delete(Guid id);
    Task<Job> GetJobById(Guid id);
    Task<List<Job>> GetJobsByBusinessName(string businessName);
    Task<List<Job>> GetJobsByBusinessField(Guid businessFieldId);

    // Hàm filter riêng
    Task<List<Job>> FilterJobsByLocation(string location);
    Task<List<Job>> FilterJobsByBudget(decimal minBudget, decimal maxBudget);

    // Hàm sắp xếp riêng
    Task<List<Job>> FilterJobsByStartDate(DateTime fromDate);

    // Hàm update status riêng
    Task<bool> UpdateJobStatus(Guid id, JobStatus newStatus);
    Task<List<Job>> GetJobsByFieldName(string fieldName);
    Task<List<Job>> GetJobsByBusinessId(Guid businessId);
    Task<List<Job>> GetJobsByStatus(JobStatus status);
    Task<List<Job>> GetJobsByStatusAndBusinessId(JobStatus status, Guid businessId);


}
