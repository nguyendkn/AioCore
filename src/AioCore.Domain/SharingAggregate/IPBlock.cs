using AioCore.Mongo;

namespace AioCore.Domain.SharingAggregate;

public class IPBlock : MongoDocument
{
    public long LocationId { get; set; }

    public long Start { get; set; }

    public long End { get; set; }
}