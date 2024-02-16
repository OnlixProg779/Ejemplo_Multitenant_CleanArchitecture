using AutoMapper;
using Moq;
using Multitenant.Application.CQRS.Products.Queries.Vms;
using Multitenant.Application.Test.Mocks;
using Multitenant.Infraestructure.Repository;

namespace Multitenant.Application.Test.CQRS.Products.Queries.GetPaginParams
{
    public class GetProductPaginParamsQueryHandlerXUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWorkBusiness> _unitOfWorkBusiness;
        public GetProductPaginParamsQueryHandlerXUnitTests()
        {
            _unitOfWorkBusiness = MockUnitOfWorkBusiness.GetUnitOfWorkBusiness();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<ProductMappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            MockProductoRepository.AddDataProductRepository(_unitOfWorkBusiness.Object.BusinessDbContext);
        }

    }
}
