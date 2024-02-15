using Multitenant.Application.Specification;
using System.Linq.Expressions;

namespace Multitenant.Application.CQRS.Products.Queries.GetPaginParams.Resources
{
    public class ProductSpecification : BaseSpecification<Domain.Bussines.Products>
    {

        public ProductSpecification(
           ProductSpecificationParams entityParams,
           List<Expression<Func<Domain.Bussines.Products, bool>>> criteria)
            : base(criteria)
        {

            ApplyPaging(entityParams.PageSize * (entityParams.PageIndex - 1), entityParams.PageSize);


        }

    }

}


