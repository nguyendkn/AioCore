using AioCore.Domain.SettingAggregate;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
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
    public override void Config(EntityTypeBuilder<DynamicAttribute> builder)
    {
        builder.Property(x => x.DataType)
            .HasConversion(v => v.ToString(), v => Enum.Parse<AttributeType>(v));
    }
}