using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicEntity : Entity
{
    public string Name { get; set; } = default!;

    [Column(TypeName = "xml")] public string Data { get; set; } = default!;

    public Guid EntityTypeId { get; set; }

    public Guid TenantId { get; set; }

    public virtual ICollection<DynamicDateValue> DateValues { get; set; } = default!;

    public virtual ICollection<DynamicFloatValue> FloatValues { get; set; } = default!;

    public virtual ICollection<DynamicGuidValue> GuidValues { get; set; } = default!;

    public virtual ICollection<DynamicIntegerValue> IntegerValues { get; set; } = default!;

    public virtual ICollection<DynamicStringValue> StringValues { get; set; } = default!;
}

public class DynamicEntityTypeConfiguration : EntityTypeConfiguration<DynamicEntity>
{
    public override void Config(EntityTypeBuilder<DynamicEntity> builder)
    {
    }
}