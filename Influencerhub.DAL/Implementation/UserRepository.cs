using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;

namespace Influencerhub.DAL.Implementation
{
    public class UserRepository : IUserRepository
    {
        private InfluencerhubDBContext _context;

        public UserRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(a => a.Email == email);
        }

        public async Task<User> GetById(Guid id)
        {
            return await _context.Users.SingleOrDefaultAsync(a => a.UserId == id);
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await _context.Users.Include(a => a.Role).SingleOrDefaultAsync(a => a.RefreshToken == refreshToken);
        }

        public async Task<User> Login(string email, string password)
        {
            return await _context.Users.Include(a => a.Role).SingleOrDefaultAsync(a => a.Email == email && a.Password == password);
        }

        public async Task<User> Update(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            // Chỉ update các trường cho phép
            existingUser.Email = user.Email;
            existingUser.RefreshToken = user.RefreshToken;
            existingUser.ExpireTimeRefreshToken = user.ExpireTimeRefreshToken;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<User> Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetByEmailVerificationToken(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);
        }

        public async Task<User> GetByResetPasswordToken(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ResetPasswordToken == token);
        }

        public async Task<User> UpdateBlockedStatus(Guid userId, bool isBlocked)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.IsBlocked = isBlocked;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Influ?> GetInfluByUserId(Guid userId)
        {
            return await _context.Influs.FirstOrDefaultAsync(i => i.UserId == userId);
        }

        public async Task<Business?> GetBusinessByUserId(Guid userId)
        {
            return await _context.Businesses.FirstOrDefaultAsync(b => b.UserId == userId);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }

        public async Task<List<User>> GetUsersByVerificationStatus(bool isVerified)
        {
            return await _context.Users.Where(u => u.IsVerified == isVerified).Include(u => u.Role).ToListAsync();
        }

        public async Task<List<User>> GetUsersByEmailVerificationStatus(bool isEmailVerified)
        {
            return await _context.Users.Where(u => u.IsEmailVerified == isEmailVerified).Include(u => u.Role).ToListAsync();
        }

        public async Task<List<User>> GetBlockedUsers()
        {
            return await _context.Users.Where(u => u.IsBlocked).Include(u => u.Role).ToListAsync();
        }


    }
}
