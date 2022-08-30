using AioCore.Domain.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicStringValue : DynamicValue<string>
{
}

public class DynamicStringValueTypeConfiguration : EntityTypeConfiguration<DynamicStringValue>
{
    private readonly string? _schema;

    public DynamicStringValueTypeConfiguration(string? schema)
    {
        _schema = schema;
    }

    public override void Config(EntityTypeBuilder<DynamicStringValue> builder)
    {
        if (!string.IsNullOrWhiteSpace(_schema))
            builder.ToTable(nameof(DynamicContext.StringValues), _schema);
    }
}