using Base.Application.Contracts.Repository;
using Base.Application.Helpers;
using Base.Domain.Common;
using Multitenant.Application.Contracts.Repository.Product;

namespace Multitenant.Application.Contracts.Repository
{
    public interface IUnitOfWorkBusiness : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntities;
        Task<int> Complete(MyTokenInformation token);
    }
}
