using Multitenant.Application.Specification;
using System.Linq.Expressions;

namespace Multitenant.Application.CQRS.Products.Queries.GetPaginParams.Resources
{
    public class ProductForCountingSpecification : BaseSpecification<Domain.Bussines.Products>
    {
        public ProductForCountingSpecification(List<Expression<Func<Domain.Bussines.Products, bool>>> criteria)
            : base(criteria)
        {

        }
    }
}
