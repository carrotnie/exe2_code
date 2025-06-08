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
        Task<ResponseDTO> SearchBusinessByName(string name);
        Task<ResponseDTO> SearchBusinessByField(string fieldName);
        Task<ResponseDTO> SearchBusinessByAddress(string address);
        Task<ResponseDTO> GetAllBusinesses();

    }
}
