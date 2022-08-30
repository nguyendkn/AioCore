using Hangfire;

namespace AioCore.Jobs;

[AutomaticRetry(Attempts = 0)]
public interface ICronJob
{
    Task<string> Run();
}