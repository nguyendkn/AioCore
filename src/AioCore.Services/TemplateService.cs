using System.Collections.Concurrent;
using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using RazorEngineCore;

namespace AioCore.Services;

public interface ITemplateService
{
    Task<string> Render(string? pathType = null, bool indexPage = false);
}

public class TemplateService : ITemplateService
{
    private readonly SettingsContext _settingsContext;
    private readonly DynamicContext _dynamicContext;
    private readonly AppSettings _appSettings;
    private readonly IHttpClientFactory _httpClient;
    private readonly IClientService _clientService;

    public TemplateService(
        SettingsContext settingsContext,
        AppSettings appSettings,
        IHttpClientFactory httpClient,
        IClientService clientService,
        DynamicContext dynamicContext)
    {
        _settingsContext = settingsContext;
        _appSettings = appSettings;
        _httpClient = httpClient;
        _clientService = clientService;
        _dynamicContext = dynamicContext;
    }

    public async Task<string> Render(string? pathType = null, bool indexPage = false)
    {
        var domain = _clientService.Host();
        if (string.IsNullOrEmpty(domain)) return string.Empty;
        var tenant = await _settingsContext.Tenants.Include(x => x!.Codes)
            .ThenInclude(x => x.EntityCodes).ThenInclude(x => x.Entity)
            .FirstOrDefaultAsync(y => y.Domain.Equals(domain));
        var settingCode = tenant?.Codes.FirstOrDefault(x => x.PathType.Equals(indexPage ? "index" : pathType));
        if (settingCode is null) return string.Empty;

        var staticCode = await GetCode(tenant, settingCode);

        // Start - Build models

        var entities = settingCode.EntityCodes.Select(x => x.Entity).ToList();
        var entityIds = entities.Select(x => x.Id);
        var entitiesData = await _dynamicContext.Entities.Where(
                x => entityIds.Contains(x.EntityId))
            .ToListAsync();

        var modelBinding = new Dictionary<string, List<Dictionary<string, object>>>();
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
        var compiledTemplate = await razorEngine.CompileAsync(staticCode, builder =>
        {
            builder.AddAssemblyReferenceByName("System.Collections"); // by name
            // builder.AddAssemblyReference(typeof(System.Collections.Generic.List<>)); // by type
        });
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