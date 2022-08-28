using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace AioCore.Services;

public interface IClientService
{
    string? Host();
}

public class ClientService : IClientService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClientService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Host() => _httpContextAccessor.HttpContext?.Request.Headers[RequestHeaders.Host].ToString();
}