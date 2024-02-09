using Multitenant.Application.Contracts.Repository.Generic;
using Multitenant.Application.Helpers;
using Multitenant.Domain.Common;

namespace Multitenant.Application.Contracts.Repository
{
    public interface IUnitOfWorkBusiness : IDisposable
    {
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntities;
        Task<int> Complete(MyTokenInformation token);
    }
}
