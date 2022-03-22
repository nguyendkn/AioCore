namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class SingleArgumentReduction : Reduction
    {
        private readonly string _arg;


        public SingleArgumentReduction(ReduceFunction function, string arg)
            : base(function)
        {
            _arg = arg;
        }


        public override string? ResultName => $"{_arg}_{Function}";


        public override IEnumerable<string> Serialize()
        {
            var ret = new List<string?>
            {
                "REDUCE",
                Function.ToString(),
                "1",
                $"@{_arg}",
                "AS",
                ResultName
            };
            return ret.ToArray()!;
        }
    }
}