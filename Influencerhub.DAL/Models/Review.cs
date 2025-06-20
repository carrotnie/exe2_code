﻿using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Influencerhub.DAL.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? InfluId { get; set; }
        [ForeignKey(nameof(InfluId))]
        public Influ? Influ { get; set; }
        public Guid? BusinessId { get; set; }
        [ForeignKey(nameof(BusinessId))]
        public Business? Business { get; set; }
        public Guid? JobId { get; set; }
        [ForeignKey(nameof(JobId))]
        public Job? Job { get; set; }
        public string? Feedback { get; set; } = null;
        public float? Rating { get; set; } = null;

        public ReviewType Type { get; set; }

    }
}
