using AioCore.Domain.DynamicAggregate;
using AioCore.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Domain.DatabaseContexts;

public class DynamicContext : DbContext
{
    public static string Schema = "aioc";

    private readonly AppSettings _appSettings;

    public DynamicContext(DbContextOptions<DynamicContext> options, AppSettings appSettings) : base(options)
    {
        _appSettings = appSettings;
    }

    public DbSet<DynamicEntity> Entities { get; set; } = default!;

    public DbSet<DynamicAttribute> Attributes { get; set; } = default!;


    public DbSet<DynamicDateValue> DateValues { get; set; } = default!;

    public DbSet<DynamicFloatValue> FloatValues { get; set; } = default!;

    public DbSet<DynamicGuidValue> GuidValues { get; set; } = default!;

    public DbSet<DynamicIntegerValue> IntegerValues { get; set; } = default!;

    public DbSet<DynamicStringValue> StringValues { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new DynamicAttributeTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DynamicDateValueTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DynamicEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DynamicFloatValueTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DynamicGuidValueTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DynamicIntegerValueTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DynamicStringValueTypeConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(_appSettings.ConnectionStrings.DefaultConnection, b =>
        {
            b.MigrationsHistoryTable("__EFMigrationsHistory", "aioc");
            b.MigrationsAssembly("AioCore.Migrations");
            b.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
        });
    }
}