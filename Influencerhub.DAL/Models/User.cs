using Messenger.DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Password { get; set; } = null!;
        public string Email { get; set; }
        public Guid? RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }
        public Boolean IsVerified { get; set; } = false; //admin duyệt nên mặc định là false nha 
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpireTimeRefreshToken { get; set; } = DateTime.UtcNow;

    }
}
