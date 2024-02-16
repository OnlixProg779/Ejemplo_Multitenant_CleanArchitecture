using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Multitenant.Application.CQRS.Products.Commands.PatchProduct;
using Multitenant.Application.CQRS.Products.Queries.Vms;
using Multitenant.Application.Test.Mocks;
using Multitenant.Infraestructure.Repository;
using Multitenant.Application.CQRS.Products.Commands.PatchProduct.Resources;
using Shouldly;

namespace Multitenant.Application.Test.CQRS.Products.Commands.PatchProduct
{
    public class PatchProductCommandHandlerXUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWorkBusiness> _unitOfWorkBusiness;
        private readonly Mock<ILogger<PatchProductCommandHandler>> _logger;
        public PatchProductCommandHandlerXUnitTests()
        {
            _unitOfWorkBusiness = MockUnitOfWorkBusiness.GetUnitOfWorkBusiness();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<ProductMappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<PatchProductCommandHandler>>();
            MockProductoRepository.AddDataProductRepository(_unitOfWorkBusiness.Object.BusinessDbContext);
        }

        [Fact]
        public async Task PatchProductCommand_InputProductPatch_ReturnProduct()
        {
            var productInput = new PatchProductCommand()
            {
                Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0NDhmNWNkZC04ZDNmLTM5ZWEtN2M2Yi0wNzQ2ODgzNWFiMTEiLCJuYW1lIjoiUHJ1ZWJhMyIsImVtYWlsIjoiZHNlZUBnbWFpbC5jb20iLCJzdWIiOiJPcmdhbml6YWNpb25Vc3VhcmlvIiwianRpIjoiMThiZTIwZDgtOGRhNC00MGVjLWI2MjctOTE3YTRlNGE1ZDU3Iiwicm9sZSI6Ik9yZ2FuaXphY2lvblVzdWFyaW8iLCJuYmYiOjE3MDc0NTA3MjgsImV4cCI6MjMyOTUzMDcyOCwiaWF0IjoxNzA3NDUwNzI4fQ.hZvYQueL-GAyd0eELVe4Q7m15Zftnoxb4nEAsmcmfX4",
                Id = new Guid("33c59e3b-9cf7-4fdb-a25b-5c1ff06248c4")
            };

            productInput.patchEntity.Add(a => a.Name, "Propiedad Modificada el nombre");

            var handler = new PatchProductCommandHandler(_unitOfWorkBusiness.Object, _mapper, _logger.Object);
            var result = await handler.Handle(productInput, CancellationToken.None);
            result.ShouldBeOfType<PatchProductResponse>();
            Assert.True(result.Name == "Propiedad Modificada el nombre");
        }
    }
}
