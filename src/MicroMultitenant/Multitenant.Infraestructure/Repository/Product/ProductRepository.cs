using Multitenant.Application.Contracts.Repository.Product;
using Multitenant.Domain.Bussines;
using Multitenant.Infraestructure.Persistence;
using Multitenant.Infraestructure.Repository.Generic;

namespace Multitenant.Infraestructure.Repository.Product
{
    public class ProductRepository: RepositoryBase<Products>, IProductRepository
    {
        public ProductRepository(BusinessDbContext context) : base(context)
        {
        }
    }
}
