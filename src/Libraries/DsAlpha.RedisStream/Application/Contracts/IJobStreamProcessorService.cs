namespace DsAlpha.RedisStream.Application.Contracts
{
    public interface IJobStreamProcessorService
    {
        Task ProcessStreamJob(string streamName);

    }
}
