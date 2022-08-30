using AioCore.Domain.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicFloatValue : DynamicValue<float>
{
}

public class DynamicFloatValueTypeConfiguration : EntityTypeConfiguration<DynamicFloatValue>
{
    private readonly string? _schema;

    public DynamicFloatValueTypeConfiguration(string? schema)
    {
        _schema = schema;
    }

    public override void Config(EntityTypeBuilder<DynamicFloatValue> builder)
    {
        if (!string.IsNullOrWhiteSpace(_schema))
            builder.ToTable(nameof(DynamicContext.FloatValues), _schema);
    }
}