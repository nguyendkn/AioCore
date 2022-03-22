using AioCore.Redis.OM.Aggregation;
using AioCore.Redis.OM.Contracts;
using AioCore.Redis.OM.Searching;
using StackExchange.Redis;

namespace AioCore.Redis.OM
{
    public class RedisConnectionProvider
    {
        private readonly IConnectionMultiplexer _mux;


        public RedisConnectionProvider(string connectionString)
        {
            var options = RedisUriParser.ParseConfigFromUri(connectionString);
            _mux = ConnectionMultiplexer.Connect(options);
        }


        public RedisConnectionProvider(RedisConnectionConfiguration connectionConfig)
        {
            _mux = ConnectionMultiplexer.Connect(connectionConfig.ToStackExchangeConnectionString());
        }


        public RedisConnectionProvider(ConfigurationOptions configurationOptions)
        {
            _mux = ConnectionMultiplexer.Connect(configurationOptions);
        }


        public RedisConnectionProvider(IConnectionMultiplexer connectionMultiplexer)
        {
            _mux = connectionMultiplexer;
        }


        public IRedisConnection Connection => new RedisConnection(_mux.GetDatabase());


        public RedisAggregationSet<T> AggregationSet<T>(int chunkSize = 100) => new(Connection, chunkSize: chunkSize);


        public IRedisCollection<T> RedisCollection<T>(int chunkSize = 100)
            where T : notnull => new RedisCollection<T>(Connection, chunkSize);
    }
}