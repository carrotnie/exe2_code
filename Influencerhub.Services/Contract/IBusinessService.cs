using Influencerhub.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IBusinessService
    {
        Task<ResponseDTO> CreateBusiness(BusinessDTO dto);

        Task<ResponseDTO> UpdateBusinessByUserId(Guid userId, BusinessUpdateDTO dto);

    }
}
