﻿using System;
using System.Threading.Tasks;
using Influencerhub.DAL.Models;

namespace Influencerhub.DAL.Contract
{
    public interface IUserRepository
    {
        Task<User> GetById(Guid id);
        Task<User> Update(User user);
        Task<User> GetByEmail(string email);
        Task<User> Login(string email, string password);
        Task<User> GetUserByRefreshToken(string refreshToken);
        Task<User> GetByEmailVerificationToken(string token);
        Task<User> Add(User user);
        Task<User> GetByResetPasswordToken(string token);
        Task<User> UpdateBlockedStatus(Guid userId, bool isBlocked);
        Task<Influ?> GetInfluByUserId(Guid userId);
        Task<Business?> GetBusinessByUserId(Guid userId);
        Task<List<User>> GetAllUsers();
        Task<List<User>> GetUsersByVerificationStatus(bool isVerified);
        Task<List<User>> GetUsersByEmailVerificationStatus(bool isEmailVerified);
        Task<List<User>> GetBlockedUsers();


    }
}
