using AioCore.Mongo;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicEntity : MongoDocument
{
    public Guid EntityId { get; set; }

    public Guid TenantId { get; set; }

    public Dictionary<string, object>? Data { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}