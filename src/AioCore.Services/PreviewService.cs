using AioCore.Blazor.Template;
using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Services;

public interface IPreviewService
{
    Task<string> Preview(string? pathType = null, bool indexPage = false);
}

public class PreviewService : IPreviewService
{
    private readonly SettingsContext _context;
    private readonly IRazorEngine _razorEngine;
    private readonly AppSettings _appSettings;
    private readonly IHttpClientFactory _httpClient;
    private readonly IClientService _clientService;

    public PreviewService(SettingsContext context,
        IRazorEngine razorEngine, AppSettings appSettings,
        IHttpClientFactory httpClient, IClientService clientService)
    {
        _context = context;
        _razorEngine = razorEngine;
        _appSettings = appSettings;
        _httpClient = httpClient;
        _clientService = clientService;
    }

    public async Task<string> Preview(string? pathType = null, bool indexPage = false)
    {
        var domain = _clientService.Host();
        if (string.IsNullOrEmpty(domain)) return string.Empty;
        var tenant = await _context.Tenants.Include(x => x.Codes)
            .FirstOrDefaultAsync(x => x.Domain.Equals(domain));
        var settingCode = tenant?.Codes.FirstOrDefault(x => x.PathType.Equals(indexPage ? "index" : pathType));
        if (settingCode is null) return string.Empty;

        var staticCode = await GetCode(tenant, settingCode);
        var template = await _razorEngine.CompileAsync(staticCode);

        var actual = await template.RunAsync(new {});
        return actual;
    }

    private async Task<string> GetCode(Entity? tenant, SettingCode code)
    {
        return code.SaveType switch
        {
            SaveType.File => $"{_appSettings.TenantConfigs.AssemblySavedFolder}\\{tenant?.Id}\\{code.Name}".ReadFile(),
            SaveType.Url => await _httpClient.CreateClient().GetStringAsync(code.Code),
            SaveType.Inline => code.Code,
            _ => string.Empty
        };
    }
}