using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Influencerhub.DAL.Models
{
    public class Business
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null!;
        public string? Address { get; set; } = null!;
        public string BusinessLicense { get; set; } // link ảnh chứng nhận doanh nghiệp mã số thuế gì đó 
        public string Logo { get; set; }

    }
}
