using System;
using Influencerhub.Common.Enum;

namespace Influencerhub.Common.DTO
{
    public class ReviewDTO
    {
        public Guid Id { get; set; }
        public Guid? JobId { get; set; }
        public string Feedback { get; set; }
        public float Rating { get; set; }
        public ReviewType Type { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}
