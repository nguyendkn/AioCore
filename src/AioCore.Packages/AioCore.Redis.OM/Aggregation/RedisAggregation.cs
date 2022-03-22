using AioCore.Redis.OM.Aggregation.AggregationPredicates;

namespace AioCore.Redis.OM.Aggregation
{
    public class RedisAggregation
    {
        public RedisAggregation(string indexName)
        {
            IndexName = indexName;
        }


        public string IndexName { get; }


        public QueryPredicate Query { get; set; } = new();


        public LimitPredicate? Limit { get; set; }


        public Stack<IAggregationPredicate> Predicates { get; } = new();


        public string[] Serialize()
        {
            var ret = new List<string> {IndexName};
            ret.AddRange(Query.Serialize());
            foreach (var predicate in Predicates)
            {
                ret.AddRange(predicate.Serialize());
            }

            if (Limit != null)
            {
                ret.AddRange(Limit.Serialize());
            }

            return ret.ToArray();
        }
    }
}