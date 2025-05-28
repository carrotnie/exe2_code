using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Influencerhub.DAL.Repository
{
    public class FieldRepository : IFieldRepository
    {
        private readonly InfluencerhubDBContext _context;

        public FieldRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<Field> CreateAsync(Field field)
        {
            _context.Fields.Add(field);
            await _context.SaveChangesAsync();
            return field;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Fields.FindAsync(id);
            if (entity == null) return false;
            _context.Fields.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Field?> UpdateAsync(Field field)
        {
            var entity = await _context.Fields.FindAsync(field.Id);
            if (entity == null) return null;
            entity.Name = field.Name;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Field?> GetByIdAsync(Guid id)
        {
            return await _context.Fields.FindAsync(id);
        }

        public async Task<List<Field>> GetByNameContainsAsync(string name)
        {
            return await _context.Fields
                .Where(f => f.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }


        public async Task<List<Field>> GetAllAsync()
        {
            return await _context.Fields.ToListAsync();
        }
    }
}
