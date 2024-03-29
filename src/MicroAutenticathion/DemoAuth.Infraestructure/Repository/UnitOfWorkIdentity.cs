﻿using Microsoft.EntityFrameworkCore;
using System.Collections;
using DemoAuth.Application.Contracts.Repository;
using DemoAuth.Infraestructure.Persistence;
using Base.Application.Helpers;
using Base.Domain.Common;
using Base.Application.Contracts.Repository;
using Base.Infraestructure.Repository;

namespace DemoAuth.Infraestructure.Repository
{
    public class UnitOfWorkIdentity : IUnitOfWorkIdentity
    {
        private readonly IdentityOrganizationDbContext _identityContext;
        protected Hashtable _repositories; 


        public UnitOfWorkIdentity(IdentityOrganizationDbContext identityContext) 
        {
            _identityContext = identityContext ??
           throw new ArgumentNullException(nameof(identityContext));
        }


        public async Task<int> Complete(MyTokenInformation token)
        {
            if (token != null)
            {

                foreach (var entry in _identityContext.ChangeTracker.Entries<BaseEntities>())
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
                return await _identityContext.SaveChangesAsync();

            }
            else return 0;
        }

        public void Dispose()
        {
            _identityContext.Dispose();
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
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _identityContext);

                _repositories.Add(type, repositoryInstance);

            }

            return (IAsyncRepository<TEntity>)_repositories[type];
        }
    }
}
