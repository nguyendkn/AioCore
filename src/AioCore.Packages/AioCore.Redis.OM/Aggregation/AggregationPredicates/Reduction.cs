namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public abstract class Reduction : IAggregationPredicate
    {
        protected Reduction(ReduceFunction function)
        {
            Function = function;
        }


        public abstract string? ResultName { get; }


        protected ReduceFunction Function { get; }


        public abstract IEnumerable<string> Serialize();
    }
}