
using AutoFixture;
using Moq;
using Multitenant.Application.Contracts.Repository.Product;
using Multitenant.Domain.Bussines;
using System.Linq.Expressions;

namespace Multitenant.Application.Test.Mocks
{
    public static class MockProductoRepository
    {
        public static Mock<IProductRepository> GetProductByIdRepository(Guid? id, List<Expression<Func<Products, object>>>? includes)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var productos = fixture.CreateMany<Products>(10).ToList();

            productos[0].Id =  new Guid("33c59e3b-9cf7-4fdb-a25b-5c1ff06248c4");

            var mockRepository = new Mock<IProductRepository>();

            mockRepository.Setup(r => r.GetByIdToCommandAsync(id, includes)).ReturnsAsync(productos.FirstOrDefault(a => a.Id == id));

            return mockRepository;
        }
    }
}
