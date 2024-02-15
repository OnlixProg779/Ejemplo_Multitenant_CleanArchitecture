using Multitenant.Application.Contracts.Repository.Generic;
using Multitenant.Application.Helpers;
using Multitenant.Domain.Common;

namespace Multitenant.Application.Contracts.Repository
{
    public interface IUnitOfWorkIdentity : IDisposable
    {
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntities;
        Task<int> Complete(MyTokenInformation token);
    }
}
