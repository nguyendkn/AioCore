using AioCore.Services.RedisService.Models;

namespace AioCore.Services.RedisService;

public interface IRedisCacheService
{
    Task<T> GetAsync<T>(RedisKey key);
    
    Task<bool> SetAsync<T>(RedisKey key, T value);
}