using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Implementation
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly InfluencerhubDBContext _context;

        public TransactionRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<Transaction> AddAsync(Transaction entity)
        {
            _context.Transactions.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<MembershipType?> GetMembershipTypeAsync(Guid membershipTypeId)
        {
            return await _context.MembershipTypes.FindAsync(membershipTypeId);
        }

        public async Task<User?> GetUserAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }

}
