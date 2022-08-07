using System.Reflection;
using AioCore.Domain.DatabaseContexts;
using AioCore.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Web.Helpers;

public static class StartupHelper
{
    public static IServiceCollection AddAioContext(this IServiceCollection services, AppSettings configuration)
    {
        services.AddDbContext<SettingsContext>(options =>
        {
            options.UseSqlServer(configuration.ConnectionStrings.DefaultConnection, b =>
            {
                b.MigrationsAssembly(typeof(AioCore.Migrations.Assembly).GetTypeInfo().Assembly.GetName().Name);
                b.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            });
        });
        return services;
    }
}