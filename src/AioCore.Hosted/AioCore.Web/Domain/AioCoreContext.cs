using AioCore.Redis.OM.RedisCore;
using AioCore.Web.Domain.AggregateModels.PageAggregate;

namespace AioCore.Web.Domain;

public class AioCoreContext : RedisContext
{
    public RedisSet<Page> Pages { get; set; } = default!;
}