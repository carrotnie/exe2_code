using System.Collections.Generic;

namespace Influencerhub.Common.DTO
{
    public class InfluUpdateDTO
    {
        public string Email { get; set; }
        public string? NickName { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Follower { get; set; }
        public string? Bio { get; set; }
        public string? LinkImage { get; set; }
        public string? Portfolio_link { get; set; }
        public string Area { get; set; } 
        public List<string> Linkmxh { get; set; }
        public List<Guid> FieldIds { get; set; }
    }
}
