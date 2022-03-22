namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class LimitPredicate : IAggregationPredicate
    {
        public long Offset { get; set; }


        public long Count { get; set; } = 100;


        public IEnumerable<string> Serialize()
        {
            return new[] {"LIMIT", Offset.ToString(), Count.ToString()};
        }
    }
}