using Influencerhub.DAL.Models;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Influencerhub.DAL.Implementation
{
    public class InfluRepository : IInfluRepository
    {
        private readonly InfluencerhubDBContext _context;
        public InfluRepository(InfluencerhubDBContext context)
        {
            _context = context;
        }

        public async Task<Influ> CreateInflu(Influ influ, User user, List<string> linkmxh)
        {

            influ.UserId = user.UserId;
            await _context.Influs.AddAsync(influ);
            await _context.SaveChangesAsync();

            if (linkmxh != null && linkmxh.Count > 0)
            {
                foreach (var link in linkmxh)
                {
                    var social = new Link
                    {
                        InfluId = influ.InfluId,
                        Linkmxh = link
                    };
                    await _context.Links.AddAsync(social);
                }
                await _context.SaveChangesAsync();
            }

            return influ;
        }

        public async Task<Influ> UpdateInflu(Influ influ, User user, List<string> linkmxh)
        {
            _context.Influs.Update(influ);
            if (user != null)
                _context.Users.Update(user);

            // Xoá toàn bộ link cũ
            var oldLinks = _context.Links.Where(l => l.InfluId == influ.InfluId);
            _context.Links.RemoveRange(oldLinks);

            if (linkmxh != null && linkmxh.Count > 0)
            {
                foreach (var link in linkmxh)
                {
                    var social = new Link
                    {
                        InfluId = influ.InfluId,
                        Linkmxh = link
                    };
                    await _context.Links.AddAsync(social);
                }
            }
            await _context.SaveChangesAsync();
            return influ;
        }
        public async Task<List<Influ>> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<Influ>();
            keyword = keyword.ToLower();
            return await _context.Influs
                .Where(i => i.Name.ToLower().Contains(keyword))
                .ToListAsync();
        }

        public async Task<List<Influ>> SearchByFieldName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<Influ>();
            keyword = keyword.ToLower();
            // Join với FreelanceField & Field để search theo tên Field
            var query = from influ in _context.Influs
                        join freelanceField in _context.FreelanceFields on influ.InfluId equals freelanceField.FreelanceId
                        join field in _context.Fields on freelanceField.FieldId equals field.Id
                        where field.Name.ToLower().Contains(keyword)
                        select influ;

            return await query.Distinct().ToListAsync();
        }

        public async Task<List<Influ>> SearchByArea(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<Influ>();
            keyword = keyword.ToLower();
            return await _context.Influs
                .Where(i => i.Area.ToLower().Contains(keyword))
                .ToListAsync();
        }

        public async Task<List<Influ>> SearchByFollower(int? minFollower, int? maxFollower)
        {
            var query = _context.Influs.AsQueryable();

            if (minFollower.HasValue)
            {
                query = query.Where(i => i.Follower.HasValue && i.Follower.Value >= minFollower.Value);
            }
            if (maxFollower.HasValue)
            {
                query = query.Where(i => i.Follower.HasValue && i.Follower.Value <= maxFollower.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<List<Influ>> GetAllInflu()
        {
            return await _context.Influs.ToListAsync();
        }

        public async Task<Influ?> GetById(Guid influId)
        {
            return await _context.Influs.FirstOrDefaultAsync(i => i.InfluId == influId);
        }

        public async Task<Influ?> GetByUserId(Guid userId)
        {
            return await _context.Influs.FirstOrDefaultAsync(i => i.UserId == userId);
        }


    }
}
