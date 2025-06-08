using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class UserVerificationUpdateDTO
    {
        public Guid UserId { get; set; }
        public bool IsVerified { get; set; }
    }
}
