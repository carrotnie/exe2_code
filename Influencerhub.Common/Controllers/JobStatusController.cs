using Influencerhub.Common.Enum;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/job-status")]
public class JobStatusController : ControllerBase
{
    private readonly IJobService _jobService;
    public JobStatusController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableJobs()
    {
        var result = await _jobService.GetJobsByStatus(JobStatus.Available);
        return Ok(result);
    }

    [HttpGet("in-progress")]
    public async Task<IActionResult> GetInProgressJobs()
    {
        var result = await _jobService.GetJobsByStatus(JobStatus.InProgress);
        return Ok(result);
    }

    [HttpGet("complete")]
    public async Task<IActionResult> GetCompleteJobs()
    {
        var result = await _jobService.GetJobsByStatus(JobStatus.Complete);
        return Ok(result);
    }

    [HttpGet("cancel")]
    public async Task<IActionResult> GetCancelJobs()
    {
        var result = await _jobService.GetJobsByStatus(JobStatus.Cancel);
        return Ok(result);
    }

    [HttpGet("registration-expired")]
    public async Task<IActionResult> GetRegistrationExpiredJobs()
    {
        var result = await _jobService.GetJobsByStatus(JobStatus.RegistrationExpired);
        return Ok(result);
    }

    [HttpGet("available/by-business/{businessId}")]
    public async Task<IActionResult> GetAvailableJobsByBusiness(Guid businessId)
    {
        var result = await _jobService.GetJobsByStatusAndBusinessId(JobStatus.Available, businessId);
        return Ok(result);
    }

    [HttpGet("in-progress/by-business/{businessId}")]
    public async Task<IActionResult> GetInProgressJobsByBusiness(Guid businessId)
    {
        var result = await _jobService.GetJobsByStatusAndBusinessId(JobStatus.InProgress, businessId);
        return Ok(result);
    }

    [HttpGet("complete/by-business/{businessId}")]
    public async Task<IActionResult> GetCompleteJobsByBusiness(Guid businessId)
    {
        var result = await _jobService.GetJobsByStatusAndBusinessId(JobStatus.Complete, businessId);
        return Ok(result);
    }

    [HttpGet("cancel/by-business/{businessId}")]
    public async Task<IActionResult> GetCancelJobsByBusiness(Guid businessId)
    {
        var result = await _jobService.GetJobsByStatusAndBusinessId(JobStatus.Cancel, businessId);
        return Ok(result);
    }

    [HttpGet("registration-expired/by-business/{businessId}")]
    public async Task<IActionResult> GetRegistrationExpiredJobsByBusiness(Guid businessId)
    {
        var result = await _jobService.GetJobsByStatusAndBusinessId(JobStatus.RegistrationExpired, businessId);
        return Ok(result);
    }
}
