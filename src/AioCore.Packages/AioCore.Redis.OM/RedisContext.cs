using StackExchange.Redis;

namespace AioCore.Redis.OM;

public class RedisContext
{
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

    private ConnectionMultiplexer Connection => _lazyConnection.Value;

    public RedisContext(RedisOptionsBuilder optionsBuilder)
    {
        _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            ConnectionMultiplexer.Connect(optionsBuilder.Connection));
    }
}