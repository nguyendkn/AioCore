using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingTenant : Entity
{
    public string Name { get; set; } = default!;

    public string Domain { get; set; } = default!;
    
    public string Title { get; set; } = default!;

    public string? Keyword { get; set; }
}