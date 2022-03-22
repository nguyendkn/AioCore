namespace AioCore.Redis.OM.Modeling
{
    public sealed class IndexedAttribute : SearchFieldAttribute
    {
        public char Separator { get; set; } = '|';

        public bool CaseSensitive { get; set; } = false;

        internal override SearchFieldType SearchFieldType => SearchFieldType.INDEXED;
    }
}