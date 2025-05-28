using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class BusinessUpdateDTO
    {
        // Chỉ update các trường cho phép
        public string? Email { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string Logo { get; set; }

        // Field
        public List<Guid> FieldIds { get; set; }

        // Đại diện
        public string RepresentativeName { get; set; }
        public string? Role { get; set; }
        public string? RepresentativeEmail { get; set; }
        public string? RepresentativePhoneNumber { get; set; }
    }
}
