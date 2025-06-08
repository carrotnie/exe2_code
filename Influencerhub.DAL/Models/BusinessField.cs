using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Models
{
    public class BusinessField
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? BusinessId { get; set; }
        [ForeignKey(nameof(BusinessId))]
        public Business? Business { get; set; }
        public Guid? FieldId { get; set; }
        [ForeignKey(nameof(FieldId))]
        public Field? Field { get; set; }
    }
}
