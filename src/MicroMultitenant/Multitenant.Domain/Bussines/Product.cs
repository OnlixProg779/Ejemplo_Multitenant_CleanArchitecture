
using Base.Domain.Common;

namespace Multitenant.Domain.Bussines
{
    public class Product : BaseEntities
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Duration { get; set; } = null!;
    }
}
