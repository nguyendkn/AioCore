using AioCore.Domain.DatabaseContexts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AioCore.Domain.DatabaseDataSeeds;

public class SettingsContextSeed
{
    public static async Task SeedAsync(
        SettingsContext context,
        ILogger<SettingsContextSeed>? logger)
    {
        var policy = CreatePolicy(logger, nameof(SettingsContextSeed));

        await policy.ExecuteAsync(async () =>
        {
            // TODO: CODE HERE
            await context.SaveChangesAsync(true);
        });
    }

    private static AsyncRetryPolicy CreatePolicy(ILogger? logger, string prefix, int retries = 3)
    {
        return Policy.Handle<SqlException>().WaitAndRetryAsync(
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