using Influencerhub.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.DTO
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid MembershipTypeId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Time { get; set; }
        public string PaymentImageLink { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
