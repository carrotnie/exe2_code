using Influencerhub.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Influencerhub.DAL.Models
{
    public class Job
    {
        [Key]
        public Guid Id { get; set; } 
        public Guid? BusinessId { get; set; }
        [ForeignKey(nameof(BusinessId))]
        public Business? Business { get; set; }
        public Guid? BusinessFieldId { get; set; }
        [ForeignKey(nameof(BusinessFieldId))]
        public BusinessField? BusinessField { get; set; }
        public string Title { get; set; } //tên dự án 
        public string? Description { get; set; } = null!; // mô tả 
        public string? KolBenefits { get; set; } = null; //quyền lợi cho kol 
        public string? Location { get; set; } = null!; // thành phố 
        public decimal Budget { get; set; }
        public Gender Gender { get; set; } = Gender.Any;
        public int Follower { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Require { get; set; } = null!; //yêu cầu ngoài dự án như ngoại hình, ngôn ngữ, phong cách 
        public JobStatus Status { get; set; } = JobStatus.Available;


    }
}
