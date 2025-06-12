using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Influencerhub.DAL.Enum;
using Influencerhub.DAL.Models;

namespace Influencerhub.DAL.Models
{
    public class PartnerShip
    {
        [Key]
        public Guid PartnerID { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(User1))]
        public Guid UserID1 { get; set; }

        [ForeignKey(nameof(User2))]
        public Guid UserID2 { get; set; }

        public FriendshipStatus Status { get; set; }

        public User? User1 { get; set; }
        public User? User2 { get; set; }
    }


}
