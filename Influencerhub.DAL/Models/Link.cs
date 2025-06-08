using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Models
{
    public class Link
    {
        [Key]
        public Guid Id { get; set; } 
        public Guid? InfluId { get; set; }

        [ForeignKey(nameof(InfluId))]
        public Influ? Influ { get; set; }

        public string? Linkmxh { get; set; } = null;

    }
}
