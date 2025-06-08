using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _jobService.GetAll();
        return Ok(result);
    }

    [HttpPost("add-job")]
    public async Task<IActionResult> Add([FromBody] JobDTO jobDto)
    {
        var result = await _jobService.Add(jobDto);
        return Ok(result);
    }

    [HttpPut("update-job/{jobId}")]
    public async Task<IActionResult> UpdateJob(Guid jobId, [FromBody] UpdateJobDTO jobDto)
    {
        var result = await _jobService.Update(jobId, jobDto);
        return Ok(result);
    }

    [HttpDelete("delete-job/{jobId}")]
    public async Task<IActionResult> DeleteJob(Guid jobId)
    {
        var result = await _jobService.Delete(jobId);
        return Ok(result);
    }

    [HttpGet("search/by-business-name")]
    public async Task<IActionResult> GetJobsByBusinessName([FromQuery] string businessName)
    {
        var result = await _jobService.GetJobsByBusinessName(businessName);
        return Ok(result);
    }

    [HttpGet("search/by-business-field/{businessFieldId}")]
    public async Task<IActionResult> GetJobsByBusinessField(Guid businessFieldId)
    {
        var result = await _jobService.GetJobsByBusinessField(businessFieldId);
        return Ok(result);
    }

    [HttpGet("filter/by-location")]
    public async Task<IActionResult> FilterJobsByLocation([FromQuery] string location)
    {
        var result = await _jobService.FilterJobsByLocation(location);
        return Ok(result);
    }

    [HttpGet("filter/by-budget")]
    public async Task<IActionResult> FilterJobsByBudget([FromQuery] decimal minBudget, [FromQuery] decimal maxBudget)
    {
        var result = await _jobService.FilterJobsByBudget(minBudget, maxBudget);
        return Ok(result);
    }

    [HttpGet("search/by-field-name")]
    public async Task<IActionResult> GetJobsByFieldName([FromQuery] string fieldName)
    {
        var result = await _jobService.GetJobsByFieldName(fieldName);
        return Ok(result);
    }

    [HttpGet("filter/by-start-date")]
    public async Task<IActionResult> FilterJobsByStartDate([FromQuery] DateTime fromDate)
    {
        var result = await _jobService.FilterJobsByStartDate(fromDate);
        return Ok(result);
    }


    [HttpPatch("{id}/update-status")]
    public async Task<IActionResult> UpdateJobStatus(Guid id, [FromBody] UpdateJobStatusDTO dto)
    {
        var result = await _jobService.UpdateJobStatus(id, dto.Status);
        return Ok(result);
    }

    [HttpGet("get-job/by-business-id/{businessId}")]
    public async Task<IActionResult> GetJobsByBusinessId(Guid businessId)
    {
        var result = await _jobService.GetJobsByBusinessId(businessId);
        return Ok(result);
    }

}
