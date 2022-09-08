using System.Reflection;
using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AioCore.Shared.Hangfire;

public static class HangfireExtension
{
    public static void AddAiocHangfire(this IServiceCollection services, string connectionString)
    {
        var storage = new SqlServerStorage(connectionString, new SqlServerStorageOptions
        {
            SchemaName = "Hangfire" + "." + Environment.MachineName,
        });
        services.AddHangfire(x => x.UseStorage(storage));
        JobStorage.Current = storage;
        services.AddHangfireServer();
    }

    public static void UseJobs(this WebApplication app, IHostEnvironment environment)
    {
        using var scope = app.Services.CreateScope();

        var jobs = scope.ServiceProvider.GetServices<ICronJob>();
        var scheduleTasks = HangfireJobs(environment).ToDictionary(t => t.Id);

        foreach (var recurringJob in jobs)
        {
            var name = recurringJob.GetType().Name;

            if (scheduleTasks.TryGetValue(name, out var exp))
            {
                RecurringJob.AddOrUpdate(name, () => recurringJob.Run(), exp.CronExpression);
            }
        }
    }

    public static void UseHangfire(this WebApplication app)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            DashboardTitle = "Hangfire",
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = "admin",
                    Pass = "admin"
                }
            },
            IgnoreAntiforgeryToken = true
        });
    }

    private static IEnumerable<HangfireJob> HangfireJobs(IHostEnvironment environment)
    {
        var instance = new List<HangfireJob>();
        var environmentName = environment.EnvironmentName;
        var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        var appSettingsPath = Path.Combine(assemblyPath!, $"appsettings.{environmentName}.json");
        new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath)
            .Build().Bind("Jobs", instance);
        return instance;
    }
}