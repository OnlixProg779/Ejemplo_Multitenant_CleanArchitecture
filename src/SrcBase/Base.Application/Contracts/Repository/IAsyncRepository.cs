using Base.Application.Contracts.Specification;
using Base.Application.CQRS.Commands;
using Base.Domain.Common;
using System.Linq.Expressions;

namespace Base.Application.Contracts.Repository
{
    public interface IAsyncRepository<T> where T : BaseEntities
    {
        Task<ChangeActivatorsResponse> ChangeActive(T entity, bool? command);

        Task<T> GetByIdToCommandAsync(Guid? id, List<Expression<Func<T, object>>> includes = null);

        void AddEntity(T entity);
        void UpdateEntity(T entity);

        Task<T> GetFirstWithSpec(ISpecification<T> spec, bool disableTracking = true);
        Task<List<T>> GetAllWithSpec(ISpecification<T> spec, bool disableTracking = true);
        Task<int> CountAsync(ISpecification<T> spec);

    }

}
