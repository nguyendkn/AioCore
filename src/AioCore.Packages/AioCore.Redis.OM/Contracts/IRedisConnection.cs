﻿namespace AioCore.Redis.OM.Contracts
{
    public interface IRedisConnection : IDisposable
    {
        RedisReply Execute(string command, params string[] args);

        Task<RedisReply> ExecuteAsync(string command, params string[] args);
    }
}