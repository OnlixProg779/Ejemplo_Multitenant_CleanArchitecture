using Microsoft.Extensions.Logging;
using Moq;
using Multitenant.Application.CQRS.Products.Commands.ChangeActivators;
using Multitenant.Application.CQRS.Products.Commands.ChangeActivators.Resources;
using Multitenant.Application.Test.Mocks;
using Multitenant.Infraestructure.Repository;
using Shouldly;

namespace Multitenant.Application.Test.CQRS.Products.Commands.ChangeActivators
{
    public class ProductChangeActivatorsCommandHandlerXUnitTests
    {
        private readonly Mock<UnitOfWorkBusiness> _unitOfWorkBusiness;
        private readonly Mock<ILogger<ProductChangeActivatorsCommandHandler>> _logger;
        public ProductChangeActivatorsCommandHandlerXUnitTests()
        {
            _unitOfWorkBusiness = MockUnitOfWorkBusiness.GetUnitOfWorkBusiness();

            _logger = new Mock<ILogger<ProductChangeActivatorsCommandHandler>>();
            MockProductoRepository.AddDataProductRepository(_unitOfWorkBusiness.Object.BusinessDbContext);
        }

        [Fact]
        public async Task ProductChangeActivatorsCommand_InputTrueActivator_ReturnProductChangeActivatorsResponse()
        {
            var input = new ProductChangeActivatorsCommand()
            {
                Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0NDhmNWNkZC04ZDNmLTM5ZWEtN2M2Yi0wNzQ2ODgzNWFiMTEiLCJuYW1lIjoiUHJ1ZWJhMyIsImVtYWlsIjoiZHNlZUBnbWFpbC5jb20iLCJzdWIiOiJPcmdhbml6YWNpb25Vc3VhcmlvIiwianRpIjoiMThiZTIwZDgtOGRhNC00MGVjLWI2MjctOTE3YTRlNGE1ZDU3Iiwicm9sZSI6Ik9yZ2FuaXphY2lvblVzdWFyaW8iLCJuYmYiOjE3MDc0NTA3MjgsImV4cCI6MjMyOTUzMDcyOCwiaWF0IjoxNzA3NDUwNzI4fQ.hZvYQueL-GAyd0eELVe4Q7m15Zftnoxb4nEAsmcmfX4",
                Id = new Guid("33c59e3b-9cf7-4fdb-a25b-5c1ff06248c4"),
                Active = true
            };
            var handler = new ProductChangeActivatorsCommandHandler(_unitOfWorkBusiness.Object, _logger.Object);
            var result = await handler.Handle(input,CancellationToken.None);
            
            result.ShouldBeOfType<ProductChangeActivatorsResponse>();
            Assert.True(result.NewValue);
        }

        [Fact]
        public async Task ProductChangeActivatorsCommand_InputFalseActivator_ReturnProductChangeActivatorsResponse()
        {
            var input = new ProductChangeActivatorsCommand()
            {
                Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0NDhmNWNkZC04ZDNmLTM5ZWEtN2M2Yi0wNzQ2ODgzNWFiMTEiLCJuYW1lIjoiUHJ1ZWJhMyIsImVtYWlsIjoiZHNlZUBnbWFpbC5jb20iLCJzdWIiOiJPcmdhbml6YWNpb25Vc3VhcmlvIiwianRpIjoiMThiZTIwZDgtOGRhNC00MGVjLWI2MjctOTE3YTRlNGE1ZDU3Iiwicm9sZSI6Ik9yZ2FuaXphY2lvblVzdWFyaW8iLCJuYmYiOjE3MDc0NTA3MjgsImV4cCI6MjMyOTUzMDcyOCwiaWF0IjoxNzA3NDUwNzI4fQ.hZvYQueL-GAyd0eELVe4Q7m15Zftnoxb4nEAsmcmfX4",
                Id = new Guid("33c59e3b-9cf7-4fdb-a25b-5c1ff06248c4"),
                Active = false
            };
            var handler = new ProductChangeActivatorsCommandHandler(_unitOfWorkBusiness.Object, _logger.Object);
            var result = await handler.Handle(input, CancellationToken.None);

            result.ShouldBeOfType<ProductChangeActivatorsResponse>();
            Assert.False(result.NewValue);
        }
    }
}
