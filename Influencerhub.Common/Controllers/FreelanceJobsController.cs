﻿using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/freelance-jobs")]
public class FreelanceJobsController : ControllerBase
{
    private readonly IFreelanceJobService _freelanceJobService;

    public FreelanceJobsController(IFreelanceJobService freelanceJobService)
    {
        _freelanceJobService = freelanceJobService;
    }

    [Authorize(Roles = "Freelancer")]
    [HttpPost("apply-job")]
    public async Task<IActionResult> ApplyForJob([FromBody] ApplyJobDTO dto)
    {
        var result = await _freelanceJobService.ApplyForJob(dto.JobId, dto.FreelanceId);
        return Ok(result);
    }

    [Authorize(Roles = "Business")]
    [HttpGet("{jobId}/list-influencers-apply-job")]
    public async Task<IActionResult> GetApplicantsByJob(Guid jobId)
    {
        var result = await _freelanceJobService.GetApplicantsByJob(jobId);
        return Ok(result);
    }

    [Authorize(Roles = "Business")]
    [HttpPost("approve-influencer-job")]
    public async Task<IActionResult> ApproveInfluencerForJob([FromBody] ApproveInfluencerJobDTO dto)
    {
        var result = await _freelanceJobService.ApproveInfluencerForJob(dto.FreelanceJobId);
        return Ok(result);
    }

    [Authorize(Roles = "Business")]
    [HttpPost("confirm-complete-job")]
    public async Task<IActionResult> ConfirmCompleteJob([FromBody] ApproveInfluencerJobDTO dto)
    {
        var result = await _freelanceJobService.ConfirmCompleteJob(dto.FreelanceJobId);
        return Ok(result);
    }

}
