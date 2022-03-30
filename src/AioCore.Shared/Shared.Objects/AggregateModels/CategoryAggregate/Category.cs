using AioCore.Mongo.Driver.MongoCore;

namespace Shared.Objects.AggregateModels.CategoryAggregate;

public class Category : MongoDocument
{
    public string Name { get; set; } = default!;
    
    public string? Timestamp { get; set; }

    public DateTimeOffset TimestampOffset { get; set; }
}