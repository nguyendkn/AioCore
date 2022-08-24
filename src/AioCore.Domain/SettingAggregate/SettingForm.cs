using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingForm : Entity
{
    public Guid? EntityId { get; set; }
    
    public string Name { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(EntityId))]
    public SettingEntity? Entity { get; set; }

    public ICollection<SettingFormAttribute> Attributes { get; set; } = default!;
}

public class SettingFormAttribute : Entity
{
    public int Order { get; set; }
    
    public Guid FormId { get; set; }

    public int ColSpan { get; set; } = 12;

    public Guid? AttributeId { get; set; }
    
    public string DisplayName { get; set; } = default!;

    [ForeignKey(nameof(FormId))] public SettingForm Form { get; set; } = default!;

    [ForeignKey(nameof(AttributeId))] public SettingAttribute? Attribute { get; set; } = default!;

    public void Update(Guid? attributeId, string? displayName)
    {
        AttributeId = attributeId ?? AttributeId;
        DisplayName = displayName ?? DisplayName;
    }
}