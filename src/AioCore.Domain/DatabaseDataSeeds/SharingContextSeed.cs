using AioCore.Domain.DatabaseContexts;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Polly;
using Polly.Retry;

namespace AioCore.Domain.DatabaseDataSeeds;

public class SharingContextSeed
{
    public static async Task SeedAsync(
        SharingContext context,
        ILogger<SharingContextSeed>? logger)
    {
        var policy = CreatePolicy(logger, nameof(SharingContextSeed));

        await policy.ExecuteAsync(async () =>
        {
        });
    }

    private static AsyncRetryPolicy CreatePolicy(ILogger? logger, string prefix, int retries = 3)
    {
        return Policy.Handle<MongoException>().WaitAndRetryAsync(
            retryCount: retries,
            sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
            onRetry: (exception, timeSpan, retry, ctx) =>
            {
                logger?.LogWarning(exception,
                    "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}",
                    prefix, exception.GetType().Name, exception.Message, retry, retries);
            }
        );
    }
}