using MediatR;
using Multitenant.Application.CQRS.Commands.Products.CreateProduct.Resources;

namespace Multitenant.Application.CQRS.Commands.Products.CreateProduct

{
    public class CreateProductCommand: CreateProductRequest, IRequest<CreateProductResponse>
    {
        public string? Token { get; set; }


    }
}
