using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Jobs;
using AioCore.Notion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AioCore.Services.BackgroundJobs;

public class NotionJob : ICronJob
{
    private readonly ILogger<NotionJob> _logger;
    private readonly SettingsContext _context;
    private readonly INotionClient _notionClient;

    public NotionJob(ILogger<NotionJob> logger,
        SettingsContext context, INotionClient notionClient)
    {
        _logger = logger;
        _context = context;
        _notionClient = notionClient;
    }

    public async Task<string> Run()
    {
        _logger.LogInformation("Starting job: {Job}", nameof(NotionJob));
        // Start Code

        var entities = await _context.Entities.Where(x => x.DataSource.Equals(DataSource.Notion)).ToListAsync();
        foreach (var entity in entities)
        {
            if (string.IsNullOrEmpty(entity.SourcePath)) return default!;
            var sourcePath = entity.SourcePath.Split("|");

            var database = sourcePath.First();
            var token = sourcePath.Last();
            var tmp = await _notionClient.QueryAsync(token, database);
        }

        // End Code
        _logger.LogInformation("Started job: {Job}", nameof(NotionJob));
        return await Task.FromResult($"Started job {nameof(NotionJob)}");
    }
}