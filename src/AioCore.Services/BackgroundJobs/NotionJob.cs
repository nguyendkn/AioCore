using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.DynamicAggregate;
using AioCore.Domain.SettingAggregate;
using AioCore.Mongo;
using AioCore.Services.NotionService;
using AioCore.Shared.Extensions;
using AioCore.Shared.Hangfire;
using AioCore.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AioCore.Services.BackgroundJobs;

public class NotionJob : ICronJob
{
    private readonly ILogger<NotionJob> _logger;
    private readonly INotionClient _notionClient;
    private readonly SettingsContext _settingsContext;
    private readonly DynamicContext _dynamicContext;
    private readonly AppSettings _appSettings;

    public NotionJob(ILogger<NotionJob> logger,
        SettingsContext settingsContext, INotionClient notionClient,
        DynamicContext dynamicContext, AppSettings appSettings)
    {
        _logger = logger;
        _settingsContext = settingsContext;
        _notionClient = notionClient;
        _dynamicContext = dynamicContext;
        _appSettings = appSettings;
    }

    public async Task<string> Run()
    {
        _logger.LogInformation("Starting job: {Job}", nameof(NotionJob));
        // Start Code

        var entities = await _settingsContext.Entities
            .Where(x => x.DataSource.Equals(DataSource.Notion)).ToListAsync();
        foreach (var entity in entities)
        {
            if (string.IsNullOrEmpty(entity.SourcePath)) return default!;
            var sourcePath = entity.SourcePath.Split("|");

            var database = sourcePath.First();
            var token = sourcePath.Last();
            var data = await _notionClient.QueryAsync(token, database);
            foreach (var dictionary in data)
            {
                var id = dictionary.FirstOrDefault(x => x.Key.Equals("Id")).Value.ToString();
                var any = await _dynamicContext.Entities.Collection(entity.Name.WithTenant(entity.TenantId))
                    .AnyAsync(x => x.Id.Equals(id!.ToGuid()));
                if (!string.IsNullOrEmpty(id) && any)
                {
                    var dynamicEntity = await _dynamicContext.Entities
                        .Collection(entity.Name.WithTenant(entity.TenantId))
                        .FirstOrDefaultAsync(x => x.Id.Equals(id));
                    if (dynamicEntity is null) continue;
                    dynamicEntity.Data = dictionary;
                    await _dynamicContext.Entities.Collection(entity.Name.WithTenant(entity.TenantId))
                        .UpdateAsync(id.ToGuid(), dynamicEntity);
                    _logger.LogInformation("Modified entity: {EntityId}", id);
                }
                else
                {
                    if (id == null) continue;
                    await _dynamicContext.Entities.Collection(entity.Name.WithTenant(entity.TenantId))
                        .AddAsync(new DynamicEntity
                        {
                            Id = id.ToGuid(),
                            EntityId = entity.Id,
                            TenantId = entity.TenantId,
                            Data = dictionary
                        });
                    _logger.LogInformation("Created entity: {EntityId}", id);
                }
            }
        }

        // End Code
        _logger.LogInformation("Started job: {Job}", nameof(NotionJob));
        return await Task.FromResult($"Started job {nameof(NotionJob)}");
    }
}