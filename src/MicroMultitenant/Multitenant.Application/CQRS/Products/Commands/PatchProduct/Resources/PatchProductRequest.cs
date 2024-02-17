namespace Multitenant.Application.CQRS.Products.Commands.PatchProduct.Resources
{
    public class PatchProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
    }
}
