using System.Linq.Expressions;
using AioCore.Redis.OM.Common;

namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class TwoArgumentReduction : Reduction
    {
        private readonly string? _arg1;
        private readonly string? _arg2;


        public TwoArgumentReduction(ReduceFunction func, MethodCallExpression expression)
            : base(func)
        {
            _arg1 = ExpressionParserUtilities.GetOperandString(expression.Arguments[1]);
            _arg2 = ExpressionParserUtilities.GetOperandString(expression.Arguments[2]);
        }


        public override string? ResultName => $"{_arg1?[1..]}_{Function}_{_arg2}";


        public override IEnumerable<string> Serialize()
        {
            var ret = new List<string?>
            {
                "REDUCE",
                Function.ToString(),
                "2",
                _arg1,
                _arg2,
                "AS",
                ResultName
            };
            return ret.ToArray()!;
        }
    }
}