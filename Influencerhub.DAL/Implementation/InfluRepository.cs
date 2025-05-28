using Influencerhub.DAL.Models;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

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
    }
}
