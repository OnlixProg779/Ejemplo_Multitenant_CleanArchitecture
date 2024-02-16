using AutoMapper;
using Moq;
using Multitenant.Application.CQRS.Products.Queries.GetById;
using Multitenant.Application.CQRS.Products.Queries.Vms;
using Multitenant.Application.Test.Mocks;
using Multitenant.Infraestructure.Repository;
using Shouldly;

namespace Multitenant.Application.Test.CQRS.Products.Queries.GetById
{
    public class GetProductByIdQueryHandlerXUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWorkBusiness> _unitOfWorkBusiness;
        public GetProductByIdQueryHandlerXUnitTests()
        {
            _unitOfWorkBusiness = MockUnitOfWorkBusiness.GetUnitOfWorkBusiness();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<ProductMappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            MockProductoRepository.AddDataProductRepository(_unitOfWorkBusiness.Object.BusinessDbContext);
        }

        [Fact]
        public async Task GetProductByIdQuery_InputId_ReturnProductVm()
        {
            var handler = new GetProductByIdQueryHandler(_unitOfWorkBusiness.Object, _mapper);
            var id = new Guid("33c59e3b-9cf7-4fdb-a25b-5c1ff06248c4");
            var result = await handler.Handle(new GetProductByIdQuery(id) { Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0NDhmNWNkZC04ZDNmLTM5ZWEtN2M2Yi0wNzQ2ODgzNWFiMTEiLCJuYW1lIjoiUHJ1ZWJhMyIsImVtYWlsIjoiZHNlZUBnbWFpbC5jb20iLCJzdWIiOiJPcmdhbml6YWNpb25Vc3VhcmlvIiwianRpIjoiMThiZTIwZDgtOGRhNC00MGVjLWI2MjctOTE3YTRlNGE1ZDU3Iiwicm9sZSI6Ik9yZ2FuaXphY2lvblVzdWFyaW8iLCJuYmYiOjE3MDc0NTA3MjgsImV4cCI6MjMyOTUzMDcyOCwiaWF0IjoxNzA3NDUwNzI4fQ.hZvYQueL-GAyd0eELVe4Q7m15Zftnoxb4nEAsmcmfX4" }, CancellationToken.None);

            result.ShouldBeOfType<ProductVm>();
        }
    }
}
