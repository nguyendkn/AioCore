using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace AioCore.Services;

public interface IClientService
{
    string? IP();
    
    string? Host();

    string RequestUrl();
}

public class ClientService : IClientService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public ClientService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? IP() => _contextAccessor.HttpContext?.Request.Headers[RequestHeaders.XForwardedFor].ToString();

    public string? Host() => _contextAccessor.HttpContext?.Request.Headers[RequestHeaders.Host].ToString();

    public string RequestUrl() => _contextAccessor.HttpContext?.Request.Path!;
}