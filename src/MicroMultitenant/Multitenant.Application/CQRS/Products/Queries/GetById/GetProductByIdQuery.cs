using MediatR;
using Multitenant.Application.CQRS.Products.Queries.GetById.Resources;
using Multitenant.Application.CQRS.Products.Queries.Vms;

namespace Multitenant.Application.CQRS.Products.Queries.GetById

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
