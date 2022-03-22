namespace AioCore.Redis.OM.Modeling
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public abstract class SearchFieldAttribute : RedisFieldAttribute
    {
        public bool Sortable { get; set; }

        public bool Aggregatable { get; set; }

        public bool Normalize { get; set; } = true;

        public string? JsonPath { get; set; }

        public int CascadeDepth { get; set; }

        internal abstract SearchFieldType SearchFieldType { get; }
    }
}