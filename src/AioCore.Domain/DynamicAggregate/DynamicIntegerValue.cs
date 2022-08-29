using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicIntegerValue : DynamicValue<int>
{
}

public class DynamicIntegerValueTypeConfiguration : EntityTypeConfiguration<DynamicIntegerValue>
{
    public override void Config(EntityTypeBuilder<DynamicIntegerValue> builder)
    {
    }
}