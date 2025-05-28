using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.Enum
{
    public enum BusinessCode
    {
        Success = 200,
        InputRequired = 400,
        UnknownServerError = 500,
        NotFound = 404,
        ExpireToken = 405,
    }
}
