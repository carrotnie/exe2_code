using Influencerhub.Common.DTO;
using System;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface ITransactionService
    {
        Task<ResponseDTO> ApproveTransactionAsync(Guid transactionId, Guid adminId);
        Task<ResponseDTO> CancelTransactionAsync(Guid transactionId, Guid adminId, string? reason = null);
        Task<ResponseDTO> GetAllTransactions();
        Task<ResponseDTO> GetTransactionsByUser(Guid userId);
    }
}
