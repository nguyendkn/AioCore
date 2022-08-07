using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicValue : Entity
{
    public Guid AttributeId { get; set; }

    public Guid EntityId { get; set; }

    public Guid EntityTypeId { get; set; }

    public string? StringValue { get; set; }

    public double? NumberValue { get; set; }

    public string? ConstantValue { get; set; }

    public DateTime DateTimeValue { get; set; }

    public Guid GuidValue { get; set; }

    public virtual DynamicAttribute Attribute { get; set; } = default!;

    public virtual DynamicEntity Entity { get; set; } = default!;
}