using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Enum
{
    public enum MessageStatus
    {
        //public string Status { get; set; } = "sent"; // "sent", "delivered", "read"
        SENT = 0,
        Delivered = 1,
        Read = 2,
    }
}
