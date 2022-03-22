using AioCore.Redis.OM.Searching;

namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class AggregateSortBy : IAggregationPredicate
    {
        public AggregateSortBy(string property, SortDirection direction, int? max = null)
        {
            Property = property;
            Direction = direction;
            Max = max;
        }


        public string Property { get; set; }


        public SortDirection Direction { get; set; }


        public int? Max { get; set; }


        public IEnumerable<string> Serialize()
        {
            var numArgs = Max.HasValue ? 4 : 2;
            var ret = new List<string?>
                {"SORTBY", numArgs.ToString(), $"@{Property}", Direction == SortDirection.Ascending ? "ASC" : "DESC"};
            if (!Max.HasValue) return ret.ToArray()!;
            ret.Add("MAX");
            ret.Add(Max.ToString());

            return ret.ToArray()!;
        }
    }
}