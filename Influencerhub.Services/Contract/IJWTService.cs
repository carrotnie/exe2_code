using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Influencerhub.DAL.Models;

namespace Influencerhub.Services.Contract
{
    public interface IJWTService
    {
        public string GenerateAccessToken(User user);
        public string GenerateRefreshToken();
    }
}
