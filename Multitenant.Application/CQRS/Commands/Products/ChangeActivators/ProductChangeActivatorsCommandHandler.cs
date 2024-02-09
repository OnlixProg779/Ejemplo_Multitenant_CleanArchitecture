using MediatR;
using Microsoft.Extensions.Logging;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.CQRS.Commands.Products.ChangeActivators.Resources;
using Multitenant.Application.Helpers;

namespace Multitenant.Application.CQRS.Commands.Products.ChangeActivators

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

            var entityFromRepo = await _unitOfWork.Repository<Domain.Bussines.Products>().GetByIdToCommandAsync(request.Id);

            var respo = await _unitOfWork.Repository<Domain.Bussines.Products>().ChangeActive(entityFromRepo, request.Active);
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
