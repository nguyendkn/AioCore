using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.DynamicAggregate;
using AioCore.Domain.SettingAggregate;
using AioCore.Jobs;
using AioCore.Notion;
using AioCore.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AioCore.Services.BackgroundJobs;

public class NotionJob : ICronJob
{
    private readonly ILogger<NotionJob> _logger;
    private readonly INotionClient _notionClient;
    private readonly SettingsContext _settingsContext;
    private readonly DynamicContext _dynamicContext;

    public NotionJob(ILogger<NotionJob> logger,
        SettingsContext settingsContext, INotionClient notionClient,
        DynamicContext dynamicContext)
    {
        _logger = logger;
        _settingsContext = settingsContext;
        _notionClient = notionClient;
        _dynamicContext = dynamicContext;
    }

    public async Task<string> Run()
    {
        _logger.LogInformation("Starting job: {Job}", nameof(NotionJob));
        // Start Code

        var entities = await _settingsContext.Entities.Where(x => x.DataSource.Equals(DataSource.Notion)).ToListAsync();
        foreach (var entity in entities)
        {
            if (string.IsNullOrEmpty(entity.SourcePath)) return default!;
            var sourcePath = entity.SourcePath.Split("|");

            var database = sourcePath.First();
            var token = sourcePath.Last();
            var data = await _notionClient.QueryAsync(token, database);
            if (data is null) continue;
            foreach (var dictionary in data)
            {
                var id = dictionary.FirstOrDefault(x => x.Key.Equals("Id")).Value.ToString();
                if (!string.IsNullOrEmpty(id) && await _dynamicContext.Entities.AnyAsync(x => x.Id.Equals(id)))
                {
                    _logger.LogInformation("Modified entity: {EntityId}", id);
                    var dynamicEntity = await _dynamicContext.Entities.FirstOrDefaultAsync(x => x.Id.Equals(id));
                    if (dynamicEntity is null) continue;
                    dynamicEntity.Data = dictionary;
                    await _dynamicContext.Entities.UpdateAsync(id, dynamicEntity);
                }
                else
                {
                    _logger.LogInformation("Created entity: {EntityId}", id);
                    if (id != null)
                        await _dynamicContext.Entities.AddAsync(new DynamicEntity
                        {
                            Id = id.ToGuid(),
                            EntityId = entity.Id,
                            TenantId = entity.TenantId,
                            Data = dictionary
                        });
                }
            }
        }

        // End Code
        _logger.LogInformation("Started job: {Job}", nameof(NotionJob));
        return await Task.FromResult($"Started job {nameof(NotionJob)}");
    }
}