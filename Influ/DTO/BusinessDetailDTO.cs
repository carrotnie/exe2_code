using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class BusinessDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string BusinessLicense { get; set; }
        public string Logo { get; set; }
        public RepresentativeDTO? Representative { get; set; }
        public List<JobDTO> JobsAvailable { get; set; } = new();
        public List<JobDTO> JobsInProgress { get; set; } = new();
        public List<JobDTO> JobsComplete { get; set; } = new();
        public float? AverageRating { get; set; }
    }

}
