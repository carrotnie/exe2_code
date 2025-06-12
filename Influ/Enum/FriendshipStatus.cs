using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Enum
{
    public enum FriendshipStatus
    {
        //public string Status { get; set; } = "pending"; // pending, accepted, blocked
        Pending = 0,
        Accepted = 1,
        Blocked = 2,
    }
}
