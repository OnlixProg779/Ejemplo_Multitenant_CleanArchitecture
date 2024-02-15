namespace Multitenant.Application.CQRS.Products.Commands.ChangeActivators.Resources
{
    public class ProductChangeActivatorsResponse
    {
        public int ResponseChange { get; set; } = 0;
        public bool? NewValue { get; set; }
        public List<string>? ResponseMessage { get; set; }
    }
}
