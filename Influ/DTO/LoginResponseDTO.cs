using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;

        public Guid UserId { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? InfluId { get; set; }
        public Guid? BusinessId { get; set; }
    }
}

