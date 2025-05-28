using Messenger.DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public Guid? MembershipId { get; set; }
        [ForeignKey(nameof(MembershipId))]
        public Membership? Membership { get; set; }
        public decimal? Amount { get; set; } // tiền 
        public DateTime? Time { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Unpaid;

    }
}
