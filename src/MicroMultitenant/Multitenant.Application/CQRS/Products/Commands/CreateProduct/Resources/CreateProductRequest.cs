namespace Multitenant.Application.CQRS.Products.Commands.CreateProduct.Resources
{
    public class CreateProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
    }
}
