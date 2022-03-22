namespace AioCore.Redis.OM.Searching.Query
{
    public class RedisSortBy : QueryOption
    {
        public string? Field { get; set; } = string.Empty;


        public SortDirection Direction { get; set; }


        internal override IEnumerable<string?> SerializeArgs
        {
            get
            {
                var dir = Direction == SortDirection.Ascending ? "ASC" : "DESC";
                return new[]
                {
                    "SORTBY",
                    Field,
                    dir,
                };
            }
        }
    }
}