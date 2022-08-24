using AioCore.Domain.SettingAggregate;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Domain.DatabaseContexts;

public class SettingsContext : DbContext
{
    public const string Schema = "Settings";

    public SettingsContext(DbContextOptions<SettingsContext> options) : base(options)
    {
    }

    public DbSet<SettingAttribute> Attributes { get; set; } = default!;

    public DbSet<SettingEntity> Entities { get; set; } = default!;

    public DbSet<SettingFeature> Features { get; set; } = default!;

    public DbSet<SettingTenant> Tenants { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        new SettingAttribute().ModelCreating<SettingAttribute>(modelBuilder, Schema);
    }
}