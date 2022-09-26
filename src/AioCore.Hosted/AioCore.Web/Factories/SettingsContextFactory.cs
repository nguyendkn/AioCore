using AioCore.Domain.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AioCore.Web.Factories;

public class SettingsContextFactory : IDesignTimeDbContextFactory<SettingsContext>
{
    public SettingsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SettingsContext>();
        optionsBuilder.UseSqlServer("Server=.;Data Source=aioc", b =>
        {
            b.MigrationsHistoryTable("__EFMigrationsHistory", SettingsContext.Schema);
            b.MigrationsAssembly(Assembly.Name);
            b.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
        });

        return new SettingsContext(optionsBuilder.Options);
    }
}