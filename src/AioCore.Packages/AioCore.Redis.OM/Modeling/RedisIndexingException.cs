namespace AioCore.Redis.OM.Modeling
{
    public class RedisIndexingException : Exception
    {
        public RedisIndexingException(string message)
            : base(message)
        {
        }
    }
}