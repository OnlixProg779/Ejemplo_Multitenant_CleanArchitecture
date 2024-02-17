using Base.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.CQRS.Products.Commands.ChangeActivators.Resources;

namespace Multitenant.Application.CQRS.Products.Commands.ChangeActivators

{
    public class ProductChangeActivatorsCommandHandler : IRequestHandler<ProductChangeActivatorsCommand, ProductChangeActivatorsResponse>
    {

        private readonly IUnitOfWorkBusiness _unitOfWork;
        private readonly ILogger<ProductChangeActivatorsCommandHandler> _logger; // Para registrar la transaccion

        public ProductChangeActivatorsCommandHandler(
            IUnitOfWorkBusiness unitOfWork,
            ILogger<ProductChangeActivatorsCommandHandler> logger
            )
        {
            _unitOfWork = unitOfWork ??
             throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ??
             throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProductChangeActivatorsResponse> Handle(ProductChangeActivatorsCommand request, CancellationToken cancellationToken)
        {
            var respToken = new MyTokenInformation(request.Token);

            var entityFromRepo = await _unitOfWork.Repository<Domain.Bussines.Product>().GetByIdToCommandAsync(request.Id);

            var respo = (ProductChangeActivatorsResponse) await _unitOfWork.Repository<Domain.Bussines.Product>().ChangeActive(entityFromRepo, request.Active);
            respo.ResponseChange = await _unitOfWork.Complete(respToken);
            if (respo.ResponseChange >= 1)
            {
                _logger.LogInformation("Change Editable se realizo correctamente");
            }
            else
            {
                _logger.LogError("Change Editable No se realizo");
            }
            return respo;
        }
    }
}
