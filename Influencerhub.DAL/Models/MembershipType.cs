using Influencerhub.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Models
{
    public class MembershipType
    {
        [Key]
        public Guid Id { get; set; } 
        public string? Name { get; set; }
        public PremiumType Type { get; set; } = PremiumType.Free;
        public decimal? Price { get; set; }
        public string? Description { get; set; }

    }
}
