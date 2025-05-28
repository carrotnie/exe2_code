using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Models
{
    public class Influ 
    {
        [Key]
        public Guid InfluId { get; set; } = Guid.NewGuid();
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string? NickName { get; set; } = null;
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; } = null;
        public int? Follower { get; set; } = null;
        public string? Bio { get; set; } = null;
        public string CCCD { get; set; }
        public string LinkImage { get; set; }
        public string? Portfolio_link { get; set; } = null;
    }
}
