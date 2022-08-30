using AioCore.Domain.DatabaseContexts;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicAttribute : Entity
{
    public string Name { get; set; } = default!;

    public AttributeType DataType { get; set; }

    public Guid EntityTypeId { get; set; }
}

public class DynamicAttributeTypeConfiguration : EntityTypeConfiguration<DynamicAttribute>
{
    private readonly string? _schema;

    public DynamicAttributeTypeConfiguration(string? schema)
    {
        _schema = schema;
    }
    
    public override void Config(EntityTypeBuilder<DynamicAttribute> builder)
    {
        if (!string.IsNullOrWhiteSpace(_schema))
            builder.ToTable(nameof(DynamicContext.Attributes), _schema);
        builder.Property(x => x.DataType)
            .HasConversion(v => v.ToString(), v => Enum.Parse<AttributeType>(v));
    }
}