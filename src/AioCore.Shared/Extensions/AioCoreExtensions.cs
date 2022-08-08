using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AioCore.Shared.Extensions;

public static class AioCoreExtensions
{
    public static IServiceCollection AddMapper<TProfile>(this IServiceCollection services) where TProfile: Profile, new()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TProfile>();
        });
        services.AddSingleton(mapperConfig.CreateMapper().RegisterMap());
        return services;
    }
}