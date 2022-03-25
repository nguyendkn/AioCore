using AioCore.Mongo.OM.MongoCore;

namespace Shared.Objects.AggregateModels.PageAggregate;

public class Page : MongoDocument
{
    public string Title { get; set; } = default!;

    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;

    public string Slug { get; set; } = default!;

    public string? Template { get; set; }

    public string? Description { get; set; }

    public List<string>? Includes { get; set; }
}