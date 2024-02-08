using Microsoft.EntityFrameworkCore;
using Multitenant.Application.Contracts.Repository.Generic;
using Multitenant.Application.Contracts.Specification;
using Multitenant.Application.Helpers;
using Multitenant.Domain.Common;
using Multitenant.Infraestructure.Specification;
using System.Linq.Expressions;

namespace Multitenant.Infraestructure.Repository.Generic
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : BaseEntities
    {
        protected readonly DbContext? _context = null;

        public RepositoryBase(DbContext context)
        {
            _context = context ??
           throw new ArgumentNullException(nameof(context));
        }

        public void AddEntity(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec, true).CountAsync();
        }

        public async Task<List<T>> GetAllWithSpec(ISpecification<T> spec, bool disableTracking = true)
        {
            return await ApplySpecification(spec, disableTracking).ToListAsync();
        }

        public async Task<T> GetByIdToCommandAsync(Guid? id, List<Expression<Func<T, object>>> includes = null)
        {
            IQueryable<T>? query = null;


            query = _context.Set<T>();


            query = query.Where(a => a.Id == id);
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));// para agregar nuevas entidades al query

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstWithSpec(ISpecification<T> spec, bool disableTracking = true)
        {
            return await ApplySpecification(spec, disableTracking).FirstOrDefaultAsync();
        }

        public void UpdateEntity(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<T> ApplySpecification(ISpecification<T> spec, bool disableTracking)
        {
            if (_context != null)
            {
                return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec, disableTracking);
            }
            else
            {
                throw new ArgumentNullException($"Falta un context para realizar las solicitudes con {nameof(SpecificationEvaluator<T>)}");
            }
        }

        public async Task<int> Complete(MyTokenInformation token)
        {
            if (token != null)
            {

                foreach (var entry in _context.ChangeTracker.Entries<BaseEntities>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedBy = token.Email;
                            //entry.Entity.IdUuarui = token.Usuario;
                            break;
                        case EntityState.Modified:
                            entry.Entity.LastModifiedBy = token.Email;
                            break;
                    }
                }
                return await _context.SaveChangesAsync();

            }
            else return 0;
        }
    }
}
