using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Multitenant.Application.CQRS.Products.Commands.CreateProduct;
using Multitenant.Application.CQRS.Products.Commands.CreateProduct.Resources;
using Multitenant.Application.CQRS.Products.Queries.Vms;
using Multitenant.Application.Test.Mocks;
using Multitenant.Infraestructure.Repository;
using Shouldly;

namespace Multitenant.Application.Test.CQRS.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandlerXUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWorkBusiness> _unitOfWorkBusiness;
        private readonly Mock<ILogger<CreateProductCommandHandler>> _logger;
        public CreateProductCommandHandlerXUnitTests()
        {
            _unitOfWorkBusiness = MockUnitOfWorkBusiness.GetUnitOfWorkBusiness();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<ProductMappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<CreateProductCommandHandler>>();
            MockProductoRepository.AddDataProductRepository(_unitOfWorkBusiness.Object.BusinessDbContext);
        }

        [Fact]
        public async Task CreateProductCommand_InputProduct_ReturnProduct()
        {
            var productInput = new CreateProductCommand()
            {
                Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0NDhmNWNkZC04ZDNmLTM5ZWEtN2M2Yi0wNzQ2ODgzNWFiMTEiLCJuYW1lIjoiUHJ1ZWJhMyIsImVtYWlsIjoiZHNlZUBnbWFpbC5jb20iLCJzdWIiOiJPcmdhbml6YWNpb25Vc3VhcmlvIiwianRpIjoiMThiZTIwZDgtOGRhNC00MGVjLWI2MjctOTE3YTRlNGE1ZDU3Iiwicm9sZSI6Ik9yZ2FuaXphY2lvblVzdWFyaW8iLCJuYmYiOjE3MDc0NTA3MjgsImV4cCI6MjMyOTUzMDcyOCwiaWF0IjoxNzA3NDUwNzI4fQ.hZvYQueL-GAyd0eELVe4Q7m15Zftnoxb4nEAsmcmfX4",
                Name = "100",
                Description = "100",
                Duration = "100"
            };
            var handler = new CreateProductCommandHandler(_unitOfWorkBusiness.Object, _mapper, _logger.Object);
            var result = await handler.Handle(productInput, CancellationToken.None);
            result.ShouldBeOfType<CreateProductResponse>();
        }
    }
}
