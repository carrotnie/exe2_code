
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;

namespace Influencerhub.DAL.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private InfluencerhubDBContext _context;

    public UnitOfWork(InfluencerhubDBContext context)
    {
        _context = context;
    }


    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }
}