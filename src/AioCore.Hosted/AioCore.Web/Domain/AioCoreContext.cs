using AioCore.Redis.OM;
using AioCore.Web.Domain.AggregateModels.PageAggregate;

namespace AioCore.Web.Domain;

public class AioCoreContext : RedisContext
{
    public AioCoreContext(IRedisContextOptionsBuilder optionsBuilder) : base(optionsBuilder)
    {
    }

    public RedisSet<Page> Pages { get; set; } = default!;
}