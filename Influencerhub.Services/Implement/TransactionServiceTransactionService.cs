using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Models;
using Influencerhub.DAL.Data;
using Influencerhub.Services.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Influencerhub.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly InfluencerhubDBContext _context;

        public TransactionService(InfluencerhubDBContext context)
        {
            _context = context;
        }

        // 1. Admin duyệt giao dịch (Approve)
        public async Task<ResponseDTO> ApproveTransactionAsync(Guid transactionId, Guid adminId)
        {
            var response = new ResponseDTO();
            try
            {
                var trans = await _context.Transactions.FindAsync(transactionId);
                if (trans == null)
                    throw new Exception("Giao dịch không tồn tại!");

                if (trans.Status != TransactionStatus.Unpaid)
                    throw new Exception("Giao dịch đã được xử lý hoặc đã bị hủy!");

                var memType = await _context.MembershipTypes.FindAsync(trans.MembershipTypeId);
                if (memType == null)
                    throw new Exception("Loại gói không tồn tại!");

                trans.Status = TransactionStatus.Paid;
                await _context.SaveChangesAsync();

                var now = DateTime.UtcNow;
                DateTime? endDate = null;
                switch (memType.Type)
                {
                    case PremiumType.Month: endDate = now.AddMonths(1); break;
                    case PremiumType.Year: endDate = now.AddYears(1); break;
                }

                var membership = new Membership
                {
                    Id = Guid.NewGuid(),
                    UserId = trans.UserId,
                    MembershipTypeId = memType.Id,
                    StartDate = now,
                    EndDate = endDate
                };
                _context.Memberships.Add(membership);
                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "Duyệt giao dịch thành công, đã kích hoạt gói cho người dùng!";
                response.Data = membership;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Duyệt giao dịch thất bại: " + ex.Message;
            }
            return response;
        }

        // 2. Admin hủy giao dịch (Cancel)
        public async Task<ResponseDTO> CancelTransactionAsync(Guid transactionId, Guid adminId, string? reason = null)
        {
            var response = new ResponseDTO();
            try
            {
                var trans = await _context.Transactions.FindAsync(transactionId);
                if (trans == null)
                    throw new Exception("Giao dịch không tồn tại!");

                if (trans.Status != TransactionStatus.Unpaid)
                    throw new Exception("Giao dịch đã được xử lý hoặc đã bị hủy!");

                trans.Status = TransactionStatus.Failed;
                // Nếu muốn lưu lý do hủy, bạn có thể thêm trường Note/Reason cho Transaction

                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "Hủy giao dịch thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Hủy giao dịch thất bại: " + ex.Message;
            }
            return response;
        }

        // Lấy toàn bộ giao dịch
        public async Task<ResponseDTO> GetAllTransactions()
        {
            var response = new ResponseDTO();
            try
            {
                var transactions = await _context.Transactions
                    .OrderByDescending(x => x.Time)
                    .Select(x => new TransactionDTO
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        MembershipTypeId = x.MembershipTypeId,
                        Amount = x.Amount,
                        Time = x.Time,
                        PaymentImageLink = x.PaymentImageLink,
                        Status = x.Status
                    }).ToListAsync();

                response.IsSuccess = true;
                response.Data = transactions;
                response.Message = "Lấy danh sách giao dịch thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Lấy danh sách giao dịch thất bại: " + ex.Message;
            }
            return response;
        }

        // Lấy giao dịch theo UserId
        public async Task<ResponseDTO> GetTransactionsByUser(Guid userId)
        {
            var response = new ResponseDTO();
            try
            {
                var transactions = await _context.Transactions
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.Time)
                    .Select(x => new TransactionDTO
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        MembershipTypeId = x.MembershipTypeId,
                        Amount = x.Amount,
                        Time = x.Time,
                        PaymentImageLink = x.PaymentImageLink,
                        Status = x.Status
                    }).ToListAsync();

                response.IsSuccess = true;
                response.Data = transactions;
                response.Message = "Lấy danh sách giao dịch của người dùng thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Lấy danh sách giao dịch thất bại: " + ex.Message;
            }
            return response;
        }

    }
}
