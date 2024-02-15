using Multitenant.Application.CQRS.Products.Queries.Vms;

namespace Multitenant.Application.CQRS.Products.Commands.PatchProduct.Resources
{
    public class PatchProductResponse : ProductVm
    {
        public InfoResponseVm? Response { get; set; }

    }
}
