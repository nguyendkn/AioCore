using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicStringValue : DynamicValue<string>
{
}

public class DynamicStringValueTypeConfiguration : EntityTypeConfiguration<DynamicStringValue>
{
    public override void Config(EntityTypeBuilder<DynamicStringValue> builder)
    {
    }
}