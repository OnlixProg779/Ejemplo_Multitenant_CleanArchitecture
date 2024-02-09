﻿using AutoMapper;
using MediatR;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.CQRS.Queries.Products.Vms;
using Multitenant.Application.Helpers;

namespace Multitenant.Application.CQRS.Queries.Products.GetById

{
    public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductVm>
    {
        private readonly IUnitOfWorkBusiness _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(
            IUnitOfWorkBusiness unitOfWork, 
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork ??
           throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
           throw new ArgumentNullException(nameof(mapper));
          
        }

        public async Task<ProductVm> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var respToken = new MyTokenInformation(request.Token);

            var entity = await _unitOfWork.Repository<Domain.Bussines.Products>().GetByIdToCommandAsync(request.Id);

            var vmToReturn = _mapper.Map<ProductVm>(entity);
            vmToReturn ??= new ProductVm();

            return vmToReturn;
        }
    }
}
