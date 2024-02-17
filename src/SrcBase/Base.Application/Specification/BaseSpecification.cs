using Base.Application.Contracts.Specification;
using System.Linq.Expressions;

namespace Base.Application.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
            Criteria = new List<Expression<Func<T, bool>>>();
        }

        public BaseSpecification(List<Expression<Func<T, bool>>> criteria)
        {
            Criteria = criteria;
        }

        public List<Expression<Func<T, bool>>> Criteria { get; } 

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        public int Take { get; private set; }
        public int Skip { get; private set; }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

    }

}
