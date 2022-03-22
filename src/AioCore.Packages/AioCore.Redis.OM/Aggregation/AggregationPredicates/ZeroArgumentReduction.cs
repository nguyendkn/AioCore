namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class ZeroArgumentReduction : Reduction
    {
        public ZeroArgumentReduction(ReduceFunction function)
            : base(function)
        {
        }


        public override string? ResultName => $"{Function}";


        public override IEnumerable<string> Serialize()
        {
            var ret = new List<string?>
            {
                "REDUCE",
                Function.ToString(),
                "0",
                "AS",
                ResultName
            };
            return ret.ToArray()!;
        }
    }
}