namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public interface IAggregationPredicate
    {
        IEnumerable<string> Serialize();
    }
}