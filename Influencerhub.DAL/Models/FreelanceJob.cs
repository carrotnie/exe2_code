using Influencerhub.Common.DTO;
using Messenger.DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Models
{
    public class FreelanceJob
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? JobId { get; set; }
        [ForeignKey(nameof(JobId))]
        public Job? Job { get; set; }
        public Guid? FreelanceId { get; set; }
        [ForeignKey(nameof(FreelanceId))]
        public Influ? Influ { get; set; }
        public JobStatus status { get; set; } = JobStatus.Available;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CancelTime { get; set; } = null;

    }
}
