
namespace Multitenant.Application.Contracts.Services
{
    public interface IApplyBusinessMigrations
    {
        Task ApplyMigrations(string organizationName);
    }
}