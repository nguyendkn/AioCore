using AioCore.Mongo.OM.MongoCore;

namespace AioCore.Web.Domain.AggregateModels.PageAggregate;

public class Page : MongoDocument
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}