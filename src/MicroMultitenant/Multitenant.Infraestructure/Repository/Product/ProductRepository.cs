using Base.Infraestructure.Repository;
using Multitenant.Application.Contracts.Repository.Product;
using Multitenant.Infraestructure.Persistence;

namespace Multitenant.Infraestructure.Repository.Product
{
    public class ProductRepository: RepositoryBase<Domain.Bussines.Product>, IProductRepository
    {
        public ProductRepository(BusinessDbContext context) : base(context)
        {
        }
    }
}
