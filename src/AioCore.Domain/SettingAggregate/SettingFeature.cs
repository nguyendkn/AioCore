using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingFeature : Entity
{
    public string Name { get; set; } = default!;
}