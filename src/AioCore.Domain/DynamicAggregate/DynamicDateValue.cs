using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicDateValue : DynamicValue<DateTime>
{
}

public class DynamicDateValueTypeConfiguration : EntityTypeConfiguration<DynamicDateValue>
{
    public override void Config(EntityTypeBuilder<DynamicDateValue> builder)
    {
            
    }
}