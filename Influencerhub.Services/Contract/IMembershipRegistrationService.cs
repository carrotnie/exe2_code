using Influencerhub.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IMembershipRegistrationService
    {
        Task<TransactionDTO> RegisterMembershipAsync(RegisterMembershipRequest request);
    }

}
