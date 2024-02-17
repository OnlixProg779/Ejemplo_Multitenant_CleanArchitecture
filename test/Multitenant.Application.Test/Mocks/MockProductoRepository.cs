using AutoFixture;
using Multitenant.Domain.Bussines;
using Multitenant.Infraestructure.Persistence;

namespace Multitenant.Application.Test.Mocks
{
    public static class MockProductoRepository
    {
        /// <summary>
        /// Metodo para cargar Data en la entidad Producto
        /// </summary>
        /// <param name="businessDbContextFake"></param>
        public static void AddDataProductRepository(BusinessDbContext businessDbContextFake)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var productos = fixture.CreateMany<Product>(10).ToList();

            productos[0].Id =  new Guid("33c59e3b-9cf7-4fdb-a25b-5c1ff06248c4");

            businessDbContextFake.Products!.AddRange(productos);
            businessDbContextFake.SaveChanges();

        }
    }
}
