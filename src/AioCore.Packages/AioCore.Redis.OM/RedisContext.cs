using StackExchange.Redis;

namespace AioCore.Redis.OM;

public class RedisContext
{
    private readonly IRedisContextOptionsBuilder _optionsBuilder;
    public static List<RedisDatabase> Database = new();

    public RedisContext(IRedisContextOptionsBuilder optionsBuilder)
    {
        _optionsBuilder = optionsBuilder;
        optionsBuilder.Configure(this, this.OnConfiguring);
    }

    protected virtual void OnConfiguring()
    {
    }
}