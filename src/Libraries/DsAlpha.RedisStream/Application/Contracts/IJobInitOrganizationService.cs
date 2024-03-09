
namespace DsAlpha.RedisStream.Application.Contracts
{
    public interface IJobInitOrganizationService
    {
        Task AddToStreamAsync<T>(T item);

    }
}
