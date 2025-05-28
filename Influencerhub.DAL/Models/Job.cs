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
    public class Job
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? BusinessId { get; set; }
        [ForeignKey(nameof(BusinessId))]
        public Business? Business { get; set; }
        public string Title { get; set; } 
        public string? Description { get; set; } = null!;
        public string? Location { get; set; } = null!;
        public decimal Budget { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Require { get; set; } = null!;
        public FreelanceJobStatus Status { get; set; } = FreelanceJobStatus.NotYetConfirmed;


    }
}
