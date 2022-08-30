using AioCore.Domain.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicIntegerValue : DynamicValue<int>
{
}

public class DynamicIntegerValueTypeConfiguration : EntityTypeConfiguration<DynamicIntegerValue>
{
    private readonly string? _schema;

    public DynamicIntegerValueTypeConfiguration(string? schema)
    {
        _schema = schema;
    }

    public override void Config(EntityTypeBuilder<DynamicIntegerValue> builder)
    {
        if (!string.IsNullOrWhiteSpace(_schema))
            builder.ToTable(nameof(DynamicContext.IntegerValues), _schema);
    }
}