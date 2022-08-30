using AioCore.Domain.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicGuidValue : DynamicValue<Guid>
{
}

public class DynamicGuidValueTypeConfiguration : EntityTypeConfiguration<DynamicGuidValue>
{
    private readonly string? _schema;

    public DynamicGuidValueTypeConfiguration(string? schema)
    {
        _schema = schema;
    }

    public override void Config(EntityTypeBuilder<DynamicGuidValue> builder)
    {
        if (!string.IsNullOrWhiteSpace(_schema))
            builder.ToTable(nameof(DynamicContext.GuidValues), _schema);
    }
}