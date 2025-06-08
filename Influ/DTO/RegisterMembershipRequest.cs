using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class RegisterMembershipRequest
    {
        public Guid UserId { get; set; }
        public Guid MembershipTypeId { get; set; }
        public string PaymentImageLink { get; set; }
    }
}
