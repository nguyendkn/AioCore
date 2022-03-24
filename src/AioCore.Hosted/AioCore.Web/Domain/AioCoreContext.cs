using AioCore.Mongo.OM.MongoCore;
using AioCore.Web.Domain.AggregateModels.PageAggregate;

namespace AioCore.Web.Domain;

public class AioCoreContext : MongoContext
{
    public MongoSet<Page> Pages { get; set; } = default!;
}