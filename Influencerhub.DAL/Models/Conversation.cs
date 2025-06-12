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
    public class Conversation
    {
        [Key]
        public Guid ConversationID { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(User))]
        public Guid UserID { get; set; }
        public string ConversationName { get; set; } = null!;
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
    }


}
