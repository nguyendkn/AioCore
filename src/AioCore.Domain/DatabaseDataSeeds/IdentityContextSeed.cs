using AioCore.Domain.IdentityAggregate;
using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AioCore.Domain.DatabaseDataSeeds;

public class IdentityContextSeed
{
    public static async Task SeedAsync(
        AppSettings appSettings,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<IdentityContextSeed>? logger)
    {
        var policy = CreatePolicy(logger, nameof(IdentityContextSeed));

        await policy.ExecuteAsync(async () =>
        {
            var user = await userManager.FindByEmailAsync(appSettings.DefaultUser.Email);
            
            if (!await roleManager.Roles.AnyAsync())
            {
                foreach (var role in appSettings.DefaultRoles)
                {
                    await roleManager.CreateAsync(new Role
                    {
                        Name = role.Name
                    });
                }
            }
            
            if (user is null)
            {
                user = Activator.CreateInstance<User>();
                user.Create(appSettings.DefaultUser.Email,
                    appSettings.DefaultUser.FullName);
                await userManager.CreateAsync(
                    user, appSettings.DefaultUser.Password);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        });
    }

    private static AsyncRetryPolicy CreatePolicy(ILogger? logger, string prefix, int retries = 3)
    {
        return Policy.Handle<SqlException>().WaitAndRetryAsync(
            retryCount: retries,
            sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
            onRetry: (exception, timeSpan, retry, ctx) =>
            {
                logger?.LogWarning(exception,
                    "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}",
                    prefix, exception.GetType().Name, exception.Message, retry, retries);
            }
        );
    }
}