using AioCore.Shared.Extensions;
using StackExchange.Redis;
using RedisKey = AioCore.Services.RedisService.Models.RedisKey;

namespace AioCore.Services.RedisService;

public class RedisCacheService : IRedisCacheService
{
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

    private ConnectionMultiplexer Connection() => _lazyConnection.Value;
    private IDatabase Database(int id) => Connection().GetDatabase(id);
    
    public RedisCacheService(string connection)
    {
        _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connection));
    }
    
    public async Task<T> GetAsync<T>(RedisKey key)
    {
        var database = Database(key.Database);
        var redisValue = (await database.StringGetAsync(key.KeyName)).ToString();
        return redisValue.Deserialize<T>();
    }

    public async Task<bool> SetAsync<T>(RedisKey key, T value)
    {
        var database = Database(key.Database);
        await database.StringSetAsync(key.KeyName, value.Serialize());
        return true;
    }
}