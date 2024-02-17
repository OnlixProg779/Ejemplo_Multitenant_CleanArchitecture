using AutoMapper;
using Base.Application.CQRS.Queries.Vms;
using MediatR;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.CQRS.Products.Queries.GetPaginParams.Resources;
using Multitenant.Application.CQRS.Products.Queries.Vms;

namespace Multitenant.Application.CQRS.Products.Queries.GetPaginParams
{
    public class GetProductPaginParamsQueryHandler
    : IRequestHandler<GetProductPaginParamsQuery, PaginationVm<ProductVm>>
    {
        private readonly IUnitOfWorkBusiness _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductPaginParamsQueryHandler(
            IUnitOfWorkBusiness unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork ??
          throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
          throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PaginationVm<ProductVm>> Handle(GetProductPaginParamsQuery request, CancellationToken cancellationToken)
        {

            bool? active = request.ProductPaginParams.Active;


            var entitySpecificationParams = new ProductSpecificationParams(request.ProductPaginParams)
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };

            entitySpecificationParams._ProductPaginParams.Active = active;
            var criteria = entitySpecificationParams.GetCriteria();

            var specCount = new ProductForCountingSpecification(criteria);
            var totalEntities = await _unitOfWork.Repository<Domain.Bussines.Product>().CountAsync(specCount);

            var spec = new ProductSpecification(entitySpecificationParams, criteria);

            var entities = await _unitOfWork.Repository<Domain.Bussines.Product>().GetAllWithSpec(spec);

            var data = _mapper.Map<List<Domain.Bussines.Product>, List<ProductVm>>(entities);

            var pagination = new PaginationVm<ProductVm>(data, totalEntities, request.PageIndex, request.PageSize);

            return pagination;
        }
    }
}
