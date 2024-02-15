using Multitenant.Application.CQRS.Products.Queries.Vms;

namespace Multitenant.Application.CQRS.Products.Commands.CreateProduct.Resources
{
    public class CreateProductResponse : ProductVm
    {
        public InfoResponseVm? Response { get; set; }
    }
}
