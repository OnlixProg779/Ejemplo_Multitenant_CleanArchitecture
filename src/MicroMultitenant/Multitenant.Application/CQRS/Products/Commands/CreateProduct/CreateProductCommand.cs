using MediatR;
using Multitenant.Application.CQRS.Products.Commands.CreateProduct.Resources;

namespace Multitenant.Application.CQRS.Products.Commands.CreateProduct

{
    public class CreateProductCommand : CreateProductRequest, IRequest<CreateProductResponse>
    {
        public string? Token { get; set; }


    }
}
