using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class BusinessReviewCreateDTO
    {
        public Guid JobId { get; set; }
        public string Feedback { get; set; }
        public float Rating { get; set; }
    }

}
