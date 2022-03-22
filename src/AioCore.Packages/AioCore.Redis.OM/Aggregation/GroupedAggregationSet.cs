using System.Linq.Expressions;

namespace AioCore.Redis.OM.Aggregation
{
    public class GroupedAggregationSet<T> : RedisAggregationSet<T>
    {
        internal GroupedAggregationSet(RedisAggregationSet<T> source, Expression expression)
            : base(source, expression)
        {
        }
    }
}