using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class InfluReviewCreateDTO
    {
        public Guid FreelanceJobId { get; set; }
        public string Feedback { get; set; }
        public float Rating { get; set; }
    }

}
