using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingTenant : Entity
{
    public string Name { get; set; } = default!;

    public string Domain { get; set; } = default!;
    
    public string Title { get; set; } = default!;

    public string? Keyword { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    public ICollection<SettingCode> Codes { get; set; } = default!;
}