using Microsoft.EntityFrameworkCore;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.Contracts.Repository.Generic;
using Multitenant.Application.Contracts.Repository.Product;
using Multitenant.Application.Helpers;
using Multitenant.Domain.Common;
using Multitenant.Infraestructure.Persistence;
using Multitenant.Infraestructure.Repository.Generic;
using Multitenant.Infraestructure.Repository.Product;
using System.Collections;

namespace Multitenant.Infraestructure.Repository
{
    public class UnitOfWorkBusiness: IUnitOfWorkBusiness
    {
        private readonly BusinessDbContext _businessDbContext;
        protected Hashtable _repositories;
        public IProductRepository _ProductRepository;

        public BusinessDbContext BusinessDbContext => _businessDbContext;
        public UnitOfWorkBusiness(BusinessDbContext businessDbContext)
        {
            _businessDbContext = businessDbContext ??
           throw new ArgumentNullException(nameof(businessDbContext));
        }

        public IProductRepository ProductRepository => _ProductRepository ?? new ProductRepository(_businessDbContext);

        public async Task<int> Complete(MyTokenInformation token)
        {
            if (token != null)
            {

                foreach (var entry in _businessDbContext.ChangeTracker.Entries<BaseEntities>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedBy = token.Email;
                            break;
                        case EntityState.Modified:
                            entry.Entity.LastModifiedBy = token.Email;
                            break;
                    }
                }
                return await _businessDbContext.SaveChangesAsync();

            }
            else return 0;
        }

        public void Dispose()
        {
            _businessDbContext.Dispose();
        }

        public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntities
        {

            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _businessDbContext);

                _repositories.Add(type, repositoryInstance);

            }

            return (IAsyncRepository<TEntity>)_repositories[type];
        }
    }
}
