namespace Multitenant.Application.CQRS.Products.Queries.Vms
{
    public class InfoResponseVm
    {
        public int ResponseAction { get; set; } = 0;
        public List<string>? ResponseMessage { get; set; }
        public string? Role { get; set; }
    }
}
