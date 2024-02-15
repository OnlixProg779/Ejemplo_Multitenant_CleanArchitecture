using Multitenant.Domain.Common;


namespace Multitenant.Domain.Bussines
{
    public class Products : BaseEntities
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Duration { get; set; } = null!;
    }
}
