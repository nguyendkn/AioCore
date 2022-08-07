using AioCore.Domain.SettingAggregate;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Domain.DatabaseContexts;

public class SettingsContext : DbContext
{
    public SettingsContext(DbContextOptions<SettingsContext> options) : base(options)
    {
    }

    public DbSet<SettingEntity> Entities { get; set; } = default!;

    public DbSet<SettingFeature> Features { get; set; } = default!;
}