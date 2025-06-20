﻿using Microsoft.AspNetCore.Mvc;
using Influencerhub.Services.Contract;
using Influencerhub.Common.DTO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Influencerhub.API.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionController(ITransactionService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("approve/{id}")]
        public async Task<ResponseDTO> ApproveTransaction([FromRoute] Guid id)
        {
            return await _service.ApproveTransactionAsync(id, Guid.Empty);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("cancel/{id}")]
        public async Task<ResponseDTO> CancelTransaction([FromRoute] Guid id, [FromBody] string? reason = null)
        {
            return await _service.CancelTransactionAsync(id, Guid.Empty, reason);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ResponseDTO> GetAll()
        {
            return await _service.GetAllTransactions();
        }

        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<ResponseDTO> GetByUser([FromRoute] Guid userId)
        {
            return await _service.GetTransactionsByUser(userId);
        }


    }
}
