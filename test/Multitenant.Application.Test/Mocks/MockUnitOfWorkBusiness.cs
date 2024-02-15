
using Moq;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Domain.Bussines;
using System.Linq.Expressions;

namespace Multitenant.Application.Test.Mocks
{
    public static class MockUnitOfWorkBusiness
    {
        public static Mock<IUnitOfWorkBusiness> GetUnitOfWorkBusiness()
        {
            var mockUnitOfWorkBusiness = new Mock<IUnitOfWorkBusiness>();
            var mockProductoRepository = MockProductoRepository.GetProductByIdRepository(Guid.NewGuid(), It.IsAny<List<Expression<Func<Products, object>>>>());

            mockUnitOfWorkBusiness.Setup(r => r.ProductRepository).Returns(mockProductoRepository.Object);

            return mockUnitOfWorkBusiness;
        }

    }
}
