using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;

public interface IJobService
{
    Task<ResponseDTO> GetAll();
    Task<ResponseDTO> Add(JobDTO jobDto);
    Task<ResponseDTO> Update(Guid id, UpdateJobDTO jobDto);
    Task<ResponseDTO> Delete(Guid id);
    Task<ResponseDTO> GetJobById(Guid id);
    Task<ResponseDTO> GetJobsByBusinessName(string businessName);
    Task<ResponseDTO> GetJobsByBusinessField(Guid businessFieldId);
    Task<ResponseDTO> FilterJobsByLocation(string location);
    Task<ResponseDTO> FilterJobsByBudget(decimal minBudget, decimal maxBudget);
    Task<ResponseDTO> UpdateJobStatus(Guid id, JobStatus newStatus);

    Task<ResponseDTO> GetJobsByFieldName(string fieldName);
    Task<ResponseDTO> FilterJobsByStartDate(DateTime fromDate);
    Task<ResponseDTO> GetJobsByBusinessId(Guid businessId);

    Task<ResponseDTO> GetJobsByStatus(JobStatus status);
    Task<ResponseDTO> GetJobsByStatusAndBusinessId(JobStatus status, Guid businessId);


}
