using Influencerhub.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Contract
{
    public interface IInfluRepository
    {
        Task<Influ> CreateInflu(Influ influ, User user, List<string> linkmxh);
        Task<Influ> UpdateInflu(Influ influ, User user, List<string> linkmxh);
        Task<List<Influ>> SearchByName(string keyword);
        Task<List<Influ>> SearchByFieldName(string keyword);
        Task<List<Influ>> SearchByArea(string keyword);
        Task<List<Influ>> SearchByFollower(int? minFollower, int? maxFollower);
        Task<List<Influ>> GetAllInflu();
        Task<Influ?> GetById(Guid influId);
        Task<Influ?> GetByUserId(Guid userId);

    }
}
