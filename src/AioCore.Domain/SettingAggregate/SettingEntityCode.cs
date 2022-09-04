using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingEntityCode : Entity
{
    public Guid EntityId { get; set; }

    public Guid CodeId { get; set; }

    [ForeignKey(nameof(EntityId))]
    public SettingEntity Entity { get; set; } = default!;

    [ForeignKey(nameof(CodeId))]
    public SettingCode Code { get; set; } = default!;
}