using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Models
{
    public class Representative
    {
        [Key]
        public Guid Id { get; set; } 
        public Guid? BusinessId { get; set; }
        [ForeignKey(nameof(BusinessId))]
        public Business? Business { get; set; }
        public string RepresentativeName { get; set; }
        public string? Role { get; set; }//chức vụ 
        public string? RepresentativeEmail { get; set; }
        public string? RepresentativePhoneNumber { get; set; }
    }
}
