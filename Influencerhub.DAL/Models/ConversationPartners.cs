using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Influencerhub.DAL.Models;

namespace Influencerhub.DAL.Models
{
    public class ConversationPartners
    {
        [Key]
        public Guid ConversationPartnersID { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(User))]
        public Guid UserID { get; set; }

        [ForeignKey(nameof(Conversation))]
        public Guid ConversationID { get; set; }

        public User? User { get; set; }
        public Conversation? Conversation { get; set; }
    }


}
