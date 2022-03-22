namespace AioCore.Redis.OM.Searching.Query
{
    public sealed class RedisQuery
    {
        public RedisQuery(string? index)
        {
            Index = index;
        }


        public long Flags { get; set; } = 0;


        public string? Index { get; set; }


        public string? QueryText { get; set; } = "*";


        public SearchLimit? Limit { get; set; }


        public RedisFilter? Filter { get; set; }


        public ReturnFields? Return { get; set; }


        public RedisSortBy? SortBy { get; set; }


        internal string[] SerializeQuery()
        {
            var ret = new List<string>();
            if (string.IsNullOrEmpty(Index))
            {
                throw new ArgumentException("Index cannot be null");
            }

            ret.Add(Index);
            ret.Add(QueryText!);
            ret.AddRange(from flag in (QueryFlags[]) Enum.GetValues(typeof(QueryFlags))
                where (Flags & (long) flag) == (long) flag
                select flag.ToString());

            if (Limit != null)
            {
                ret.AddRange(Limit.SerializeArgs!);
            }

            if (Filter != null)
            {
                ret.AddRange(Filter.SerializeArgs!);
            }

            if (Return != null)
            {
                ret.AddRange(Return.SerializeArgs!);
            }

            if (SortBy != null)
            {
                ret.AddRange(SortBy.SerializeArgs!);
            }

            return ret.ToArray();
        }
    }
}