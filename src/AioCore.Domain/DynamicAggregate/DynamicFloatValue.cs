using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicFloatValue : DynamicValue<float>
{
}

public class DynamicFloatValueTypeConfiguration : EntityTypeConfiguration<DynamicFloatValue>
{
    public override void Config(EntityTypeBuilder<DynamicFloatValue> builder)
    {
    }
}