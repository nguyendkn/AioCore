using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicGuidValue : DynamicValue<Guid>
{
}

public class DynamicGuidValueTypeConfiguration : EntityTypeConfiguration<DynamicGuidValue>
{
    public override void Config(EntityTypeBuilder<DynamicGuidValue> builder)
    {
    }
}