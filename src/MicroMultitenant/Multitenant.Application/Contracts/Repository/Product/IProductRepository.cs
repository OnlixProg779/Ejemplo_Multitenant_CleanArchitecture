using Multitenant.Application.Contracts.Repository.Generic;
using Multitenant.Domain.Bussines;

namespace Multitenant.Application.Contracts.Repository.Product
{
    public interface IProductRepository : IAsyncRepository<Products>
    {
    }
}
