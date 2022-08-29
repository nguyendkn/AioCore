using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Services;

public interface IClientService
{
    string? Host();

    Task<SettingTenant> Tenant();
}

public class ClientService : IClientService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SettingsContext _settingsContext;

    public ClientService(IHttpContextAccessor httpContextAccessor, SettingsContext settingsContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _settingsContext = settingsContext;
    }

    public string? Host() => _httpContextAccessor.HttpContext?.Request.Headers[RequestHeaders.Host].ToString();

    public async Task<SettingTenant> Tenant()
    {
        return await _settingsContext.Tenants.FirstOrDefaultAsync(x => x.Domain.Equals(Host()));
    }
}