using AioCore.Domain.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicDateValue : DynamicValue<DateTime>
{
}

public class DynamicDateValueTypeConfiguration : EntityTypeConfiguration<DynamicDateValue>
{
    private readonly string? _schema;

    public DynamicDateValueTypeConfiguration(string? schema)
    {
        _schema = schema;
    }

    public override void Config(EntityTypeBuilder<DynamicDateValue> builder)
    {
        if (!string.IsNullOrWhiteSpace(_schema))
            builder.ToTable(nameof(DynamicContext.DateValues), _schema);
    }
}