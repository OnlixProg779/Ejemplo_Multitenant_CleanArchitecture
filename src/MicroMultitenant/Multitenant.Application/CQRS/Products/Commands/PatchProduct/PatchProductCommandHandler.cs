using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.CQRS.Products.Commands.PatchProduct.Resources;
using Multitenant.Application.CQRS.Products.Queries.Vms;
using Multitenant.Application.Helpers;

namespace Multitenant.Application.CQRS.Products.Commands.PatchProduct

{
    public class PatchProductCommandHandler : IRequestHandler<PatchProductCommand, PatchProductResponse>
    {
        private readonly IUnitOfWorkBusiness _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PatchProductCommandHandler> _logger;

        public PatchProductCommandHandler(
            IUnitOfWorkBusiness unitOfWork,
            IMapper mapper,
            ILogger<PatchProductCommandHandler> logger
            )
        {

            _unitOfWork = unitOfWork ??
             throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
             throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
             throw new ArgumentNullException(nameof(logger));

        }

        public async Task<PatchProductResponse> Handle(PatchProductCommand request, CancellationToken cancellationToken)
        {
            var respToken = new MyTokenInformation(request.Token);

            var entityFromRepo = await _unitOfWork.Repository<Domain.Bussines.Products>().GetByIdToCommandAsync(request.Id);

            List<string>? responseMessage = new List<string>();


            if (entityFromRepo == null)
            {
                var msg = $"El usuario no existe o esta mal referenciado";

                responseMessage.Add(msg);
                return new PatchProductResponse()
                {
                    Response = new InfoResponseVm()
                    {
                        ResponseAction = 0,
                        ResponseMessage = responseMessage,
                    },
                };
            }

            if (entityFromRepo.Active == false)
            {
                var msg = $"La Entidad esta eliminada";
                responseMessage.Add(msg);
                _logger.LogInformation(msg);
                return new PatchProductResponse()
                {
                    Response = new InfoResponseVm()
                    {
                        ResponseAction = 0,
                        ResponseMessage = responseMessage,
                    },
                };
            }

            var entityToPatch = _mapper.Map<ProductPropertyPatch>(entityFromRepo);

            request.patchEntity.ApplyTo(entityToPatch);

            _mapper.Map(entityToPatch, entityFromRepo);

            _unitOfWork.Repository<Domain.Bussines.Products>().UpdateEntity(entityFromRepo);

            var result = await _unitOfWork.Complete(respToken);

            var entityReturn = _mapper.Map<PatchProductResponse>(entityFromRepo);

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
