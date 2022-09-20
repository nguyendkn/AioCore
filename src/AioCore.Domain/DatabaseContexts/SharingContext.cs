using AioCore.Domain.SharingAggregate;
using AioCore.Mongo;
using AioCore.Mongo.Abstracts;

namespace AioCore.Domain.DatabaseContexts;

public class SharingContext : MongoContext
{
    public SharingContext(IMongoContextBuilder modelBuilder) : base(modelBuilder)
    {
    }

    public MongoSet<IPBlock> IPBlocks { get; set; } = default!;

    public MongoSet<IPLocation> IPLocations { get; set; } = default!;

    public MongoSet<Location> Locations { get; set; } = default!;
}