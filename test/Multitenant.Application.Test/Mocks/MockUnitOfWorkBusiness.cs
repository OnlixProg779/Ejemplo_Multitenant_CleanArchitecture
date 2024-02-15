using Moq;
using Multitenant.Application.Contracts.Repository;

namespace Multitenant.Application.Test.Mocks
{
    public static class MockUnitOfWorkBusiness
    {
        public static Mock<IUnitOfWorkBusiness> GetUnitOfWorkBusiness()
        {
            var mockUnitOfWorkBusiness = new Mock<IUnitOfWorkBusiness>();
            var mockProductoRepository = MockProductoRepository.GetProductByIdRepository();

            mockUnitOfWorkBusiness.Setup(r => r.ProductRepository).Returns(mockProductoRepository.Object);

            return mockUnitOfWorkBusiness;
        }

    }
}
