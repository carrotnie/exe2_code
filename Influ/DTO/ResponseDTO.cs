using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Influencerhub.Common.Enum;

namespace Influencerhub.Common.DTO
{
    public class ResponseDTO
    {
        public object Data { get; set; }
        public BusinessCode BusinessCode { get; set; } = BusinessCode.Success;
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
    }
}
