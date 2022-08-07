using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicAttribute : Entity
{
    public string Name { get; set; } = default!;

    public Guid EntityId { get; set; }

    [ForeignKey(nameof(EntityId))]
    public DynamicEntity Entity { get; set; } = default!;
}