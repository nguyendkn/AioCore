using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Services.NotionService;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using RazorEngineCore;

namespace AioCore.Services;

public interface ITemplateService
{
    Task<string> Render(string host);
}

public class TemplateService : ITemplateService
{
    private readonly SettingsContext _settingsContext;
    private readonly DynamicContext _dynamicContext;
    private readonly AppSettings _appSettings;
    private readonly IClientService _clientService;
    private readonly IHttpClientFactory _httpClient;
    private readonly INotionClient _notionClient;

    public TemplateService(
        SettingsContext settingsContext,
        AppSettings appSettings,
        IHttpClientFactory httpClient,
        DynamicContext dynamicContext,
        INotionClient notionClient,
        IClientService clientService)
    {
        _settingsContext = settingsContext;
        _appSettings = appSettings;
        _httpClient = httpClient;
        _dynamicContext = dynamicContext;
        _notionClient = notionClient;
        _clientService = clientService;
    }

    public async Task<string> Render(string host)
    {
        if (string.IsNullOrEmpty(_clientService.Host())) return string.Empty;
        var tenant = await _settingsContext.Tenants.Include(x => x!.Codes)
            .ThenInclude(x => x.EntityCodes).ThenInclude(x => x.Entity)
            .FirstOrDefaultAsync(y => y.Domain.Equals(_clientService.Host()));
        var settingCode = tenant?.Codes.FirstOrDefault(x =>
            x.PathType.Equals(host.Contains('/') ? "index" : host.Split('/').First()));
        if (settingCode is null) return string.Empty;

        var staticCode = await GetCode(tenant, settingCode);

        // Start - Build models

        var entities = settingCode.EntityCodes.Select(x => x.Entity).ToList();
        var entityIds = entities.Select(x => x.Id);
        var entitiesData = await _dynamicContext.Entities.Where(
                x => entityIds.Contains(x.EntityId))
            .ToListAsync();
        var modelBinding = new Dictionary<string, List<Dictionary<string, object>>>();

        var singleRecord = entitiesData.FirstOrDefault(x => x.Data
            .Select(y => y.Value.ToString())
            .Any(z => !string.IsNullOrEmpty(z) && z.Equals(host.Split('/')[1])));
        if (singleRecord?.Data != null)
        {
            var id = singleRecord.Data.FirstOrDefault(x => x.Key.Equals("Id")).Value.ToString();
            if (string.IsNullOrEmpty(id)) return default!;
            var settingEntity = entities.FirstOrDefault(x => x.Id.Equals(singleRecord.EntityId));
            var page = await _notionClient.GetPageAsync(settingEntity?.SourcePath?.Split('|').Last(), id);
            if (page is null) return default!;
            singleRecord.Data["StaticHtml"] = page.ToHtml();
            modelBinding.Add("SingleValue", new List<Dictionary<string, object>> { singleRecord.Data });
        }

        foreach (var group in entitiesData.GroupBy(x => x.EntityId))
        {
            var entity = entities.FirstOrDefault(x => x.Id.Equals(group.Key));
            var dictionary = group.OrderByDescending(x => x.CreatedAt).ToList()
                .Select(x => x.Data!).ToList();
            if (entity is not null && dictionary.Any())
            {
                modelBinding.TryAdd(entity.Name, dictionary.ToList());
            }
        }

        // End - Build models
        var razorEngine = new RazorEngine();
        var compiledTemplate = await razorEngine.CompileAsync(staticCode,
            builder => { builder.AddAssemblyReferenceByName("System.Collections"); });
        var result = await compiledTemplate.RunAsync(modelBinding);
        return result;
    }

    private async Task<string> GetCode(Entity? tenant, SettingCode code)
    {
        return code.SaveType switch
        {
            SaveType.File => $"{_appSettings.TenantConfigs.AssemblySavedFolder}/{tenant?.Id}/{code.Name}".ReadFile(),
            SaveType.Url => await _httpClient.CreateClient().GetStringAsync(code.Code),
            SaveType.Inline => code.Code,
            _ => string.Empty
        };
    }
}