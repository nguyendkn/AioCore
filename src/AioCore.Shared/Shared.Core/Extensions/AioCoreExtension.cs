using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Types.ValueObjects;

namespace Shared.Core.Extensions;

public static class AioCoreExtension
{
    public static void AddAioCore(this IServiceCollection services, IConfiguration configuration,
        Action<IServiceCollection, AppSettings> actionServices)
    {
        var appSettings = new AppSettings();
        configuration.Bind(appSettings);
        services.AddSingleton(appSettings);
        services.AddRazorPages();
        services.AddServerSideBlazor();
        actionServices?.Invoke(services, appSettings);
    }

    public static WebApplication UseAioCore(this WebApplication app)
    {
        app.UseStaticFiles();
        app.UseRouting();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        return app;
    }
}