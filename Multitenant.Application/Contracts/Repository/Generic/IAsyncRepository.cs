using Microsoft.EntityFrameworkCore;
using Multitenant.Application.Contracts.Specification;
using Multitenant.Application.Helpers;
using Multitenant.Domain.Common;
using System.Linq.Expressions;

namespace Multitenant.Application.Contracts.Repository.Generic
{
    public interface IAsyncRepository<T> where T : BaseEntities
    {

        Task<T> GetByIdToCommandAsync(Guid? id, List<Expression<Func<T, object>>> includes = null);

        void AddEntity(T entity);
        void UpdateEntity(T entity);

        Task<T> GetFirstWithSpec(ISpecification<T> spec, bool disableTracking = true);
        Task<List<T>> GetAllWithSpec(ISpecification<T> spec, bool disableTracking = true);
        Task<int> CountAsync(ISpecification<T> spec);

        Task<int> Complete(MyTokenInformation token);


    }

}
