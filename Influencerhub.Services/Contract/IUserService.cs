using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Influencerhub.Common.DTO;
using Influencerhub.DAL.Models;

namespace Influencerhub.Services.Contract
{
    public interface IUserService
    {

        Task<ResponseDTO> Login(UserDTO DTO);

        Task<ResponseDTO> GenerateNewToken(string refreshToken);

    }
}
