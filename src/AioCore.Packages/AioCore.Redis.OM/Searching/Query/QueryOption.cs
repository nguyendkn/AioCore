namespace AioCore.Redis.OM.Searching.Query
{
    public abstract class QueryOption
    {
        internal abstract IEnumerable<string?> SerializeArgs { get; }
    }
}