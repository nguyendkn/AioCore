namespace AioCore.Redis.OM.Modeling
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SearchableAttribute : SearchFieldAttribute
    {
        public bool NoStem { get; set; } = false;


        public string PhoneticMatcher { get; set; } = string.Empty;


        public double Weight { get; set; } = 1;


        internal override SearchFieldType SearchFieldType => SearchFieldType.TEXT;
    }
}