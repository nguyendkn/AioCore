namespace AioCore.Redis.OM.Modeling
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RedisFieldAttribute : Attribute
    {
        public string PropertyName { get; set; } = string.Empty;
    }
}