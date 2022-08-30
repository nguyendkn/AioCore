using System.Reflection;
using AioCore.Shared.ValueObjects;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
    
    public static AppSettings Configuration(IHostEnvironment environment)
    {
        var environmentName = environment.EnvironmentName;
        var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        var appSettingsPath = Path.Combine(assemblyPath!, $"appsettings.{environmentName}.json");
        var appSettings = new AppSettings();
        new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath)
            .Build().Bind(appSettings);
        return appSettings;
    }
}