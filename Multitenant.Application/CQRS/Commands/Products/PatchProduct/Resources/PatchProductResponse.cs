using Multitenant.Application.CQRS.Queries.Products.Vms;

namespace Multitenant.Application.CQRS.Commands.Products.PatchProduct.Resources
{
    public class PatchProductResponse: ProductVm
    {
        public InfoResponseVm? Response { get; set; }

    }
}
