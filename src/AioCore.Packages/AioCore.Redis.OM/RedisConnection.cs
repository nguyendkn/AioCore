using AioCore.Redis.OM.Contracts;
using StackExchange.Redis;

namespace AioCore.Redis.OM
{
    internal class RedisConnection : IRedisConnection
    {
        private readonly IDatabase _db;

        internal RedisConnection(IDatabase db)
        {
            _db = db;
        }

        public RedisReply Execute(string command, params string[] args)
        {
            var @params = args as object[];
            var result = _db.Execute(command, @params);
            return new RedisReply(result);
        }

        public async Task<RedisReply> ExecuteAsync(string command, string[] args)
        {
            var @params = args as object[];
            var result = await _db.ExecuteAsync(command, @params);
            return new RedisReply(result);
        }

        public void Dispose()
        {
        }
    }
}