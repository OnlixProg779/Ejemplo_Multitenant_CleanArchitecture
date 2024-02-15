using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Multitenant.Domain.Bussines;
using Multitenant.Domain.Common;

namespace Multitenant.Infraestructure.Persistence
{
    public class BusinessDbContext : DbContext
    {
        public virtual DbSet<Products>? Products { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BusinessDbContext(DbContextOptions<BusinessDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;

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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
          try
            {
                // TODO: Agregar una validacion para comprobar que la organizacoin exista en la parametrizacion de base.
                var connectionString = _httpContextAccessor.HttpContext?.Items["ConnectionStringLoad"]?.ToString();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseNpgsql(connectionString);
                }
            }
            catch (System.NullReferenceException)
            {
                // Es script
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(md5(((random())::text || (clock_timestamp())::text)))::uuid");
            });

            base.OnModelCreating(modelBuilder);

        }
    }
}
