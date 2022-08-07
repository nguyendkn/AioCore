using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingFeature : Entity
{
    public int Index { get; set; }
    
    public string Name { get; set; } = default!;

    public string Href { get; set; } = default!;

    public string Icon { get; set; } = default!;
}