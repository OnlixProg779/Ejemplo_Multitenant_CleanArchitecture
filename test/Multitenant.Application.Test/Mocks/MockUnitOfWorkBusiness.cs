using Microsoft.EntityFrameworkCore;
using Moq;
using Multitenant.Infraestructure.Persistence;
using Multitenant.Infraestructure.Repository;

namespace Multitenant.Application.Test.Mocks
{
    public static class MockUnitOfWorkBusiness
    {
        public static Mock<UnitOfWorkBusiness> GetUnitOfWorkBusiness()
        {
            var options = new DbContextOptionsBuilder<BusinessDbContext>()
              .UseInMemoryDatabase(databaseName: $"BusinessDbContext-{Guid.NewGuid()}")
              .Options;

            var businessDbContextFake = new BusinessDbContext(options, null);
            businessDbContextFake.Database.EnsureDeleted();
            var mockUnitOfWorkBusiness = new Mock<UnitOfWorkBusiness>(businessDbContextFake);


            return mockUnitOfWorkBusiness;
        }

    }
}
