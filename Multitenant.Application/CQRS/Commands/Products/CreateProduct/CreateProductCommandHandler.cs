using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.CQRS.Commands.Products.CreateProduct.Resources;
using Multitenant.Application.CQRS.Queries.Products.Vms;
using Multitenant.Application.Helpers;

namespace Multitenant.Application.CQRS.Commands.Products.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        private readonly IUnitOfWorkBusiness _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductCommandHandler> _logger; 

        public CreateProductCommandHandler(
            IUnitOfWorkBusiness unitOfWork, 
            IMapper mapper, 
            ILogger<CreateProductCommandHandler> logger
            )
        {
            _unitOfWork = unitOfWork ??
            throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
            throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            var respToken = new MyTokenInformation(request.Token);

            var entity = _mapper.Map<Domain.Bussines.Products>(request);
            List<string>? responseMessage = new List<string>();

            _unitOfWork.Repository<Domain.Bussines.Products>().AddEntity(entity);

            var result = await _unitOfWork.Complete(respToken);

            if (result <= 0)
            {
                throw new Exception($"No se pudo insertar el record de {nameof(Domain.Bussines.Products)}");
            }
            var msg = $"{nameof(Domain.Bussines.Products)} {entity.Id} fue creado exitosamente";
            responseMessage.Add(msg);
            _logger.LogInformation(msg);

            var entityReturn = _mapper.Map<CreateProductResponse>(entity);
            entityReturn.Response = new InfoResponseVm()
            {
                ResponseMessage = responseMessage,
                ResponseAction = result,
                Role = respToken.Role
            };
            return entityReturn;

        }
    }
}
