using System.Linq.Expressions;
using StackExchange.Redis;

namespace AioCore.Redis.OM;

public class RedisSet<TEntity> where TEntity : class
{
    private readonly IRedisContextOptionsBuilder _optionsBuilder;
    private readonly RedisContext _context;
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
    private ConnectionMultiplexer Connection => _lazyConnection.Value;
    private IDatabase Database(int id) => Connection.GetDatabase(id);

    public RedisSet(IRedisContextOptionsBuilder optionsBuilder, RedisContext context,
        Lazy<ConnectionMultiplexer> lazyConnection)
    {
        _optionsBuilder = optionsBuilder;
        _context = context;
        _lazyConnection = lazyConnection;
    }

    public async Task<TEntity> AddAsync(TEntity page)
    {
        throw new NotImplementedException();
    }

    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var redisDatabase = RedisContext.Database.FirstOrDefault(x => x.Type == typeof(TEntity));
        if (redisDatabase is null) return await Task.FromResult<TEntity>(default!);
        var database = Database(redisDatabase.Index);
        return default!;
    }
}