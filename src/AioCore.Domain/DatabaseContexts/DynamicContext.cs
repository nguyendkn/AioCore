using AioCore.Domain.DynamicAggregate;
using AioCore.Mongo;
using AioCore.Mongo.Abstracts;

namespace AioCore.Domain.DatabaseContexts;

public class DynamicContext : MongoContext
{
    public DynamicContext(IMongoContextBuilder modelBuilder) : base(modelBuilder)
    {
    }

    public MongoSet<DynamicRequest> Requests { get; set; } = default!;
    
    public MongoSet<DynamicEntity> Entities { get; set; } = default!;
}