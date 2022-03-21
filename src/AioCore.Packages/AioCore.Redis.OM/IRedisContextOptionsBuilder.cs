using StackExchange.Redis;

namespace AioCore.Redis.OM;

public interface IRedisContextOptionsBuilder
{
    Lazy<ConnectionMultiplexer> LazyConnection { get; }

    void Configure(RedisContext context, Action configAction);
}