using System;
using System.Collections.Generic;
using Influencerhub.Common.Enum;

namespace Influencerhub.Common.DTO
{
    public class InfluDetailDTO
    {
        public Guid InfluId { get; set; }
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public InfluGender Gender { get; set; }
        public string? NickName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string Area { get; set; }
        public int? Follower { get; set; }
        public string? Bio { get; set; }
        public string CCCD { get; set; }
        public string LinkImage { get; set; }
        public string? Portfolio_link { get; set; }

        public string Email { get; set; }
        public List<string> Linkmxh { get; set; } = new();
    }
}
