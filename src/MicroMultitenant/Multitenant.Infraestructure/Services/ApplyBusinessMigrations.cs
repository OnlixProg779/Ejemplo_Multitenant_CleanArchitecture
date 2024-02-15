using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Multitenant.Application.Contracts.Services;
using Multitenant.Infraestructure.Persistence;

namespace Multitenant.Infraestructure.Services
{
    public class ApplyBusinessMigrations : IApplyBusinessMigrations
    {
        private readonly IConfiguration _configuration;

        public ApplyBusinessMigrations(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ApplyMigrations(string organizationName)
        {
            var templateConnectionString = _configuration.GetConnectionString("Bussines");
            if (templateConnectionString.Contains("[_OrganizationName_]"))
            {
                var connectionString = templateConnectionString.Replace("[_OrganizationName_]", organizationName);
                var optionsBuilder = new DbContextOptionsBuilder<BusinessDbContext>();
                optionsBuilder.UseNpgsql(connectionString);

                using (var context = new BusinessDbContext(optionsBuilder.Options, null)) 
                {
                    await context.Database.MigrateAsync();
                }
            }
        }

    }
}
