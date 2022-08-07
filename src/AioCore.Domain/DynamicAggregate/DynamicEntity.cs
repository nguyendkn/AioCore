using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicEntity : Entity
{
    public string Name { get; set; } = default!;
}