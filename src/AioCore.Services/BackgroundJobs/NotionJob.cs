using AioCore.Jobs;
using Microsoft.Extensions.Logging;

namespace AioCore.Services.BackgroundJobs;

public class NotionJob : ICronJob
{
    private readonly ILogger<NotionJob> _logger;

    public NotionJob(ILogger<NotionJob> logger)
    {
        _logger = logger;
    }

    public async Task<string> Run()
    {
        _logger.LogInformation("Starting job: {Job}", nameof(NotionJob));
        _logger.LogInformation("Started job: {Job}", nameof(NotionJob));
        return await Task.FromResult($"Started job {nameof(NotionJob)}");
    }
}