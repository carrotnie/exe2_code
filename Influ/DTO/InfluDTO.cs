using Influencerhub.Common.Enum;
using System;
using System.Collections.Generic;

namespace Influencerhub.Common.DTO
{
    public class InfluDTO
    {
        // Thông tin User
        public string Email { get; set; }
        public string Password { get; set; }

        // Thông tin Influencer
        public string Name { get; set; }
        public InfluGender Gender { get; set; }
        public string? NickName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Follower { get; set; }
        public string? Bio { get; set; }
        public string CCCD { get; set; }
        public string LinkImage { get; set; }
        public string? Portfolio_link { get; set; }
        public string Area { get; set; } // <-- Thêm dòng này

        // Nhiều link mạng xã hội
        public List<string> Linkmxh { get; set; }

        // Thêm danh sách FieldId
        public List<Guid> FieldIds { get; set; }
    }
}
