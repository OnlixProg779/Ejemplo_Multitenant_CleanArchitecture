using Base.Application.Contracts.Repository;
using Base.Application.Helpers;
using Base.Domain.Common;

namespace DemoAuth.Application.Contracts.Repository
{
    public interface IUnitOfWorkIdentity : IDisposable
    {
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntities;
        Task<int> Complete(MyTokenInformation token);
    }
}
