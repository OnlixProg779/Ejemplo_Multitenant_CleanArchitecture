using AutoMapper;
using Base.Application.CQRS.Queries.Vms;
using Base.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.CQRS.Products.Commands.CreateProduct.Resources;

namespace Multitenant.Application.CQRS.Products.Commands.CreateProduct
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

            var entity = _mapper.Map<Domain.Bussines.Product>(request);
            List<string>? responseMessage = new List<string>();

            _unitOfWork.Repository<Domain.Bussines.Product>().AddEntity(entity);

            var result = await _unitOfWork.Complete(respToken);

            if (result <= 0)
            {
                throw new Exception($"No se pudo insertar el record de {nameof(Domain.Bussines.Product)}");
            }
            var msg = $"{nameof(Domain.Bussines.Product)} {entity.Id} fue creado exitosamente";
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
