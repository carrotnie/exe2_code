﻿using Influencerhub.Common.DTO;
using System;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IFreelanceJobService
    {
        Task<ResponseDTO> ApplyForJob(Guid jobId, Guid freelanceId);
        Task<ResponseDTO> GetApplicantsByJob(Guid jobId);
        Task<ResponseDTO> ApproveInfluencerForJob(Guid freelanceJobId);
        Task<ResponseDTO> ConfirmCompleteJob(Guid freelanceJobId);

    }
}
