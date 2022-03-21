namespace AioCore.Redis.OM;

public interface IRedisQueryable : IQueryable
{
}

public interface IRedisQueryable<out T> : IRedisQueryable, IQueryable<T>
{
}