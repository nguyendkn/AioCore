using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingCode : Entity
{
    public string Name { get; set; } = default!;

    public string PathFile { get; set; } = default!;

    public Guid? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))] public SettingCode? Parent { get; set; }

    public List<SettingCode> Child { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}