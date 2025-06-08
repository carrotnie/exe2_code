using Influencerhub.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class MembershipTypeDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public PremiumType Type { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
    }

}
