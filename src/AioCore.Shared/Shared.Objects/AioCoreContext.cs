using AioCore.Mongo.Driver.MongoCore;
using AioCore.Mongo.Driver.MongoCore.Abstracts;
using Shared.Objects.AggregateModels.PageAggregate;

namespace Shared.Objects;

public class AioCoreContext : MongoContext
{
    public AioCoreContext(IMongoContextBuilder builder) : base(builder)
    {
    }

    public MongoSet<Page> Pages { get; set; } = default!;
}