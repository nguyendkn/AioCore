using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.DatabaseDataSeeds;
using AioCore.Domain.IdentityAggregate;
using AioCore.Mongo;
using AioCore.Services;
using AioCore.Services.BackgroundJobs;
using AioCore.Services.GraphQueries;
using AioCore.Services.NotionService;
using AioCore.Shared.Extensions;
using AioCore.Shared.Hangfire;
using AioCore.Shared.ValueObjects;
using AioCore.Web.Providers;
using AioCore.Web.Services;
using JsonSubTypes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

namespace AioCore.Web.Helpers;

public static class StartupHelper
{
    public static void AddAioController(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
            options.SerializerSettings.Converters.Add(new JsonSubtypes());
        });
        services.AddSwaggerGenNewtonsoftSupport();
        services.AddSwaggerGen();
    }

    public static void AddAioContext(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<SettingsContext>(options =>
        {
            options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection, b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory", SettingsContext.Schema);
                b.MigrationsAssembly(Assembly.Name);
                b.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            });
        }, ServiceLifetime.Transient);
        services.AddMongoContext<DynamicContext>(appSettings.MongoConfigs);
        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection, b =>
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
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<IClaimsTransformation, ClaimsTransformation>();
        services.AddScoped<AuthenticationStateProvider, StateProvider<User>>();
    }

    public static void AddSingletonAioCore(this IServiceCollection services)
    {
        services.AddAiocNotionClient();
        services.AddSingleton<IAvatarService, AvatarService>();
    }

    public static void AddBackgroundServicesAioCore(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddAiocHangfire(appSettings.ConnectionStrings.DefaultConnection);
        services.AddScoped<ICronJob, NotionJob>();
        services.AddScoped<IGraphService, GraphService>();
    }

    public static void UseAioController(this WebApplication app)
    {
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void UseAioCoreDatabase(this WebApplication app, AppSettings appSettings)
    {
        app.MigrateDatabase<IdentityContext>((context, appServices) =>
        {
            var logger = appServices.GetService<ILogger<IdentityContextSeed>>();
            var userManager = appServices.GetRequiredService<UserManager<User>>();
            var roleManager = appServices.GetRequiredService<RoleManager<Role>>();
            IdentityContextSeed.SeedAsync(appSettings, userManager, roleManager, logger).Wait();
        });
        app.MigrateDatabase<SettingsContext>((context, appServices) =>
        {
            var logger = appServices.GetService<ILogger<SettingsContextSeed>>();
            SettingsContextSeed.SeedAsync(context, logger).Wait();
        });
    }
}