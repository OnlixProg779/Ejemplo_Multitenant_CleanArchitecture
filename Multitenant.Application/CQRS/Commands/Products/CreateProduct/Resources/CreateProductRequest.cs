
namespace Multitenant.Application.CQRS.Commands.Products.CreateProduct.Resources
{
    public class CreateProductRequest
    {
        public string? Name { get; set; } 
        public string? Description { get; set; }
        public string? Duration { get; set; } 
    }
}
