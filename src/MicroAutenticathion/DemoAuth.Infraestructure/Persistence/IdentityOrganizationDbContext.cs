using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DemoAuth.Domain.Identity;
using Base.Domain.Common;

namespace DemoAuth.Infraestructure.Persistence
{
    public class IdentityOrganizationDbContext : IdentityDbContext
    {
        public virtual DbSet<Organization>? OrganizationIdentity { get; set; }

        public IdentityOrganizationDbContext(DbContextOptions<IdentityOrganizationDbContext> options) : base(options)
        {

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntities>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.Active = true;
                        if (string.IsNullOrWhiteSpace(entry.Entity.CreatedBy)) entry.Entity.CreatedBy = "System Default Create";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        if (string.IsNullOrWhiteSpace(entry.Entity.LastModifiedBy)) entry.Entity.LastModifiedBy = "System Default Update";

                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(md5(((random())::text || (clock_timestamp())::text)))::uuid");
            });

            base.OnModelCreating(modelBuilder);

        }
    }
}
