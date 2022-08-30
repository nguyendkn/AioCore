using AioCore.Database;
using AioCore.Domain.DynamicAggregate;
using AioCore.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AioCore.Domain.DatabaseContexts;

public class DynamicContext : DbContext, IDbContextSchema
{
    public string? Schema { get; }

    public DynamicContext(DbContextOptions<DynamicContext> options, IDbContextSchema schema) : base(options)
    {
        Schema = schema?.Schema;
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
        modelBuilder.ApplyConfiguration(new DynamicAttributeTypeConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new DynamicDateValueTypeConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new DynamicEntityTypeConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new DynamicFloatValueTypeConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new DynamicGuidValueTypeConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new DynamicIntegerValueTypeConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new DynamicStringValueTypeConfiguration(Schema));
    }

    public static DynamicContext GetContext(AppSettings appSettings, string? schema = null)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DynamicContext>()
            .UseSqlServer(appSettings.ConnectionStrings.DefaultConnection
                , b =>
                {
                    b.MigrationsHistoryTable("__EFMigrationsHistory", schema);
                    b.MigrationsAssembly("AioCore.Migrations");
                }
            )
            .ReplaceService<IModelCacheKeyFactory, DbSchemaAwareModelCacheKeyFactory>()
            .ReplaceService<IMigrationsAssembly, DbSchemaAwareMigrationAssembly>();

        return new DynamicContext(optionsBuilder.Options, new DbContextSchema(schema));
    }
}