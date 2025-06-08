using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using System;
using System.Threading.Tasks;

namespace Influencerhub.Services.Implement
{
    public class MembershipRegistrationService : IMembershipRegistrationService
    {
        private readonly ITransactionRepository _transaction;

        public MembershipRegistrationService(ITransactionRepository transaction)
        {
            _transaction = transaction;
        }

        public async Task<TransactionDTO> RegisterMembershipAsync(RegisterMembershipRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PaymentImageLink))
                throw new ArgumentException("Ảnh chuyển khoản là bắt buộc!");

            var membershipType = await _transaction.GetMembershipTypeAsync(request.MembershipTypeId)
                ?? throw new Exception("Loại gói (MembershipType) không tồn tại!");

            var user = await _transaction.GetUserAsync(request.UserId)
                ?? throw new Exception("User không tồn tại!");

            decimal amount = membershipType.Price ?? 0;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                MembershipTypeId = request.MembershipTypeId,
                Amount = amount,
                Time = DateTime.UtcNow,
                PaymentImageLink = request.PaymentImageLink, 
                Status = TransactionStatus.Unpaid
            };

            var result = await _transaction.AddAsync(transaction);

            return new TransactionDTO
            {
                Id = result.Id,
                UserId = result.UserId,
                MembershipTypeId = result.MembershipTypeId,
                Amount = result.Amount,
                Time = result.Time,
                PaymentImageLink = result.PaymentImageLink,
                Status = result.Status
            };
        }
    }
}
