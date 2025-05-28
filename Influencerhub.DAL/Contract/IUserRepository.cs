using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Influencerhub.Common.DTO;
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

        Task<User> Add(User user);


    }
}
