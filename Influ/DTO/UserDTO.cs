using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class UserDTO
    {
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
    }

}
