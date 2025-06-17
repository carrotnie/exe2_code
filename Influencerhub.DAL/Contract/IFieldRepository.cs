using Influencerhub.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.DAL.Contract
{
    public interface IFieldRepository
    {
        Task<Field> CreateAsync(Field field);
        Task<bool> DeleteAsync(Guid id);
        Task<Field?> UpdateAsync(Field field);
        Task<Field?> GetByIdAsync(Guid id);
        Task<List<Field>> GetByNameContainsAsync(string name);
        Task<List<Field>> GetAllAsync();
        Task<List<Field>> GetByIdsAsync(List<Guid> ids);
        Task<List<Field>> GetFieldsByInfluId(Guid influId);


    }
}
