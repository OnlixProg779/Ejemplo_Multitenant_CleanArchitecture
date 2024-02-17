using Microsoft.EntityFrameworkCore;
using Base.Application.Contracts.Specification;
using Base.Domain.Common;

namespace Base.Application.Specification
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

            if(spec.Skip != 0 || spec.Take != 0)
            {
                inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
            }

            inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));
            return inputQuery;
        }

    }
}
