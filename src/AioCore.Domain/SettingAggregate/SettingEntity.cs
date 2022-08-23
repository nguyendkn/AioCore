using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingEntity : Entity
{
    public string? Name { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}