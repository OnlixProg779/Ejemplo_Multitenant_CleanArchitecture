using Base.Application.Specification;
using System.Linq.Expressions;

namespace Multitenant.Application.CQRS.Products.Queries.GetPaginParams.Resources
{
    public class ProductForCountingSpecification : BaseSpecification<Domain.Bussines.Product>
    {
        public ProductForCountingSpecification(List<Expression<Func<Domain.Bussines.Product, bool>>> criteria)
            : base(criteria)
        {

        }
    }
}
