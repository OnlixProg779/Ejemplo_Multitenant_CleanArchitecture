using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Multitenant.Application.CQRS.Products.Commands.PatchProduct.Resources;

namespace Multitenant.Application.CQRS.Products.Commands.PatchProduct

{
    public class PatchProductCommand : IRequest<PatchProductResponse>
    {
        public string? Token { get; set; }
        public Guid Id { get; set; }
        public JsonPatchDocument<ProductPropertyPatch> patchEntity { get; set; } = new JsonPatchDocument<ProductPropertyPatch>();
    }
}
