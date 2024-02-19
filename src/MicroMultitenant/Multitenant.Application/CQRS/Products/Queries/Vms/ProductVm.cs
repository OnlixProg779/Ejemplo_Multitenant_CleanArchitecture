namespace Multitenant.Application.CQRS.Products.Queries.Vms
{
    public class ProductVm
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public bool? Active { get; set; }

    }
}
