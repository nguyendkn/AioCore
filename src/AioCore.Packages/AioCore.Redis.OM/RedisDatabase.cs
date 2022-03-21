namespace AioCore.Redis.OM;

public class RedisDatabase
{
    public Type Type { get; set; } = default!;

    public int Index { get; set; }

    public RedisDatabase(Type type, int index)
    {
    }
}