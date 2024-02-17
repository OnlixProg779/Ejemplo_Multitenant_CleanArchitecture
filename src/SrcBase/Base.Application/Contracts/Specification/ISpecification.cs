using System.Linq.Expressions;


namespace Base.Application.Contracts.Specification
{
    public interface ISpecification<T>
    {
        List<Expression<Func<T, bool>>> Criteria { get; } 
        List<Expression<Func<T, object>>> Includes { get; }

        int Take { get; } 
        int Skip { get; } 

    }
}
