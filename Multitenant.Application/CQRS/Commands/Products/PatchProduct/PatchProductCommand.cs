using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Multitenant.Application.CQRS.Commands.Products.PatchProduct.Resources;

namespace Multitenant.Application.CQRS.Commands.Products.PatchProduct

{
    public class PatchProductCommand : IRequest<PatchProductResponse>
    {
        public string? Token { get; set; }
        public Guid Id { get; set; }
        public JsonPatchDocument<ProductPropertyPatch> patchEntity { get; set; } = new JsonPatchDocument<ProductPropertyPatch>();
    }
}
