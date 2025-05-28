using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Influencerhub.Common.DTO;
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

    }
}