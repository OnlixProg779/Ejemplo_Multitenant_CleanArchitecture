using MediatR;
using Multitenant.Application.CQRS.Queries.Products.GetById.Resources;
using Multitenant.Application.CQRS.Queries.Products.Vms;

namespace Multitenant.Application.CQRS.Queries.Products.GetById

{
    public class GetProductByIdQuery
    : GetByIdRequest, IRequest<ProductVm>
    {
        public string? Token { get; set; }

        public GetProductByIdQuery(Guid? id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}
