using Microsoft.EntityFrameworkCore;
using Multitenant.Application.Contracts.Specification;
using Multitenant.Domain.Common;

namespace Multitenant.Application.Specification
{
    public class SpecificationEvaluator<T> where T : BaseEntities
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec, bool disableTracking = true)
        {
            if (disableTracking) inputQuery = inputQuery.AsNoTracking();

            if (spec.Criteria != null)
            {
                foreach (var item in spec.Criteria)
                {
                    inputQuery = inputQuery.Where(item);
                }
            }

            inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));
            return inputQuery;
        }

    }
}
