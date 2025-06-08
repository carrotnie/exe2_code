using Influencerhub.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class JobDTO
    {
        public Guid? BusinessId { get; set; }
        public Guid? BusinessFieldId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public decimal Budget { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Require { get; set; }
        public JobStatus Status { get; set; } = JobStatus.Available;
        public string? KolBenefits { get; set; }
        public Gender Gender { get; set; } = Gender.Any;
        public int Follower { get; set; }
    }
}
