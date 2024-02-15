
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using Multitenant.Application.Contracts.Repository.Product;
using Multitenant.Domain.Bussines;
using Multitenant.Infraestructure.Persistence;
using Multitenant.Infraestructure.Repository.Product;
using System.Linq.Expressions;

namespace Multitenant.Application.Test.Mocks
{
    public static class MockProductoRepository
    {
        public static Mock<ProductRepository> GetProductByIdRepository()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var productos = fixture.CreateMany<Products>(10).ToList();

            productos[0].Id =  new Guid("33c59e3b-9cf7-4fdb-a25b-5c1ff06248c4");

            var options = new DbContextOptionsBuilder<BusinessDbContext>()
                .UseInMemoryDatabase(databaseName:$"BusinessDbContext-{Guid.NewGuid()}")
                .Options;

            var businessDbContextFake = new BusinessDbContext(options, null);
            businessDbContextFake.Products!.AddRange(productos);
            businessDbContextFake.SaveChanges();

            var mockRepository = new Mock<ProductRepository>(businessDbContextFake);

            return mockRepository;
        }
    }
}
