using AioCore.Mongo;

namespace AioCore.Domain.DynamicAggregate;

public class DynamicRequest : MongoDocument
{
    public Guid Tenant { get; set; }

    public string? Url { get; set; }

    public string? IP { get; set; }

    public long? IPLong { get; set; }

    public string? Country { get; set; }

    public string? Province { get; set; }
}