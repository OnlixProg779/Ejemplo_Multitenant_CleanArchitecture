using Base.Application.Contracts.Repository;

namespace Multitenant.Application.Contracts.Repository.Product
{
    public interface IProductRepository : IAsyncRepository<Domain.Bussines.Product>
    {
    }
}
