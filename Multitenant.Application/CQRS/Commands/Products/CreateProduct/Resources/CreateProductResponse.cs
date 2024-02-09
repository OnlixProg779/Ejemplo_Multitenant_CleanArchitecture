
using Multitenant.Application.CQRS.Queries.Products.Vms;

namespace Multitenant.Application.CQRS.Commands.Products.CreateProduct.Resources
{
    public class CreateProductResponse: ProductVm
    {
        public InfoResponseVm? Response { get; set; }
    }
}
