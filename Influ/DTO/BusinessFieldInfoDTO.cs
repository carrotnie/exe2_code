using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class BusinessFieldInfoDTO
    {
        public Guid BusinessFieldId { get; set; }
        public Guid FieldId { get; set; }
        public string FieldName { get; set; }
    }

}
