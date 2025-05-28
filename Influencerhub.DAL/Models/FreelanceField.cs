using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Models
{
    public class FreelanceField
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? FreelanceId { get; set; }
        [ForeignKey(nameof(FreelanceId))]
        public Influ? Influ { get; set; }
        public Guid? FieldId { get; set; }
        [ForeignKey(nameof(FieldId))]
        public Field? Field { get; set; }
    }
}
