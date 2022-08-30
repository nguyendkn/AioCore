using AioCore.Blazor.Template;
using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.DatabaseDataSeeds;
using AioCore.Domain.IdentityAggregate;
using AIoCore.Migrations.Migrations;
using AioCore.Services;
using AioCore.Shared.Extensions;
using AioCore.Shared.ValueObjects;
using AioCore.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Web.Helpers;

public static class StartupHelper
{
    public static void AddAioController(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddControllers();
        services.AddSwaggerGen();
    }

    public static void AddAioContext(this IServiceCollection services, AppSettings configuration)
    {
        services.AddDbContext<SettingsContext>(options =>
        {
            options.UseSqlServer(configuration.ConnectionStrings.DefaultConnection, b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory", SettingsContext.Schema);
                b.MigrationsAssembly(Assembly.Name);
                b.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            });
        }, ServiceLifetime.Transient);
        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseSqlServer(configuration.ConnectionStrings.DefaultConnection, b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory", IdentityContext.Schema);
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
            .AddRoles<Role>()
            .AddEntityFrameworkStores<IdentityContext>();
    }

    public static void AddScopedAioCore(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IPreviewService, PreviewService>();
    }

    public static void AddSingletonAioCore(this IServiceCollection services)
    {
        services.AddSingleton<IAvatarService, AvatarService>();
        services.AddSingleton<IRazorEngine, RazorEngine>();
    }

    public static void UseAioController(this WebApplication app)
    {
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void UseAioCoreDatabase(this WebApplication app)
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
    }
}