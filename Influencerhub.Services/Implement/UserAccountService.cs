using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;


namespace Influencerhub.Services.Implement
{
    public class UserAccountService : IUserAccountService
    {
        public Task<ResponseDTO> CreateUserAccount(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
