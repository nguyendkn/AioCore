using System.Reflection;
using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.DatabaseDataSeeds;
using AioCore.Domain.IdentityAggregate;
using AioCore.Shared.Extensions;
using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
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
                b.MigrationsAssembly(Assembly.Name);
                b.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            });
        });
        services.AddDbContext<DynamicContext>(options =>
        {
            options.UseSqlServer(configuration.ConnectionStrings.DefaultConnection, b =>
            {
                b.MigrationsAssembly(AioCore.Migrations.Assembly.Name);
                b.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            });
        });
        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseSqlServer(configuration.ConnectionStrings.DefaultConnection, b =>
            {
                b.MigrationsAssembly(Assembly.Name);
                b.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            });
        });
        services.AddDefaultIdentity<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<IdentityContext>();
        return services;
    }

    public static WebApplication UseAioCore(this WebApplication app)
    {
        app.MigrateDatabase<IdentityContext>((context, appServices) =>
        {
            var logger = appServices.GetService<ILogger<IdentityContextSeed>>();
            IdentityContextSeed.SeedAsync(context, logger).Wait();
        });
        app.MigrateDatabase<SettingsContext>((context, appServices) =>
        {
            var logger = appServices.GetService<ILogger<SettingsContextSeed>>();
            SettingsContextSeed.SeedAsync(context, logger).Wait();
        });
        return app;
    }
}