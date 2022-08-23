using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingAttribute : Entity
{
    public string Name { get; set; } = default!;

    public AttributeType DataType { get; set; }

    public Guid EntityId { get; set; }

    [ForeignKey(nameof(EntityId))]
    public SettingEntity Entity { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    public void Update(string name)
    {
        Name = name;
        ModifiedAt = DateTime.Now;
    }
}

public enum AttributeType
{
    Undefined,
    Text,
    Number,
    DateTime,
    Select
}