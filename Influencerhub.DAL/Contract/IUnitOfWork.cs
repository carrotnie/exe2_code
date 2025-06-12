namespace Influencerhub.DAL.Contract;

public interface IUnitOfWork
{
    public Task<int> SaveChangeAsync();
}