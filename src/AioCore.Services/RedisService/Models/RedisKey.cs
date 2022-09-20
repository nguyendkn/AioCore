namespace AioCore.Services.RedisService.Models;

public class RedisKey
{
    public int Database { get; set; }

    public string? KeyName { get; set; }
}