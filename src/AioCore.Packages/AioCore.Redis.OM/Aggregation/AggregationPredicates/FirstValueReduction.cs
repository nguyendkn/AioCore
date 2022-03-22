using System.Linq.Expressions;
using AioCore.Redis.OM.Common;
using AioCore.Redis.OM.Searching;

namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class FirstValueReduction : Reduction
    {
        private readonly string? _returnArg;
        private readonly int _numArgs = 1;
        private readonly string? _sortArg = string.Empty;
        private readonly SortDirection? _direction;


        public FirstValueReduction(MethodCallExpression exp)
            : base(ReduceFunction.FIRST_VALUE)
        {
            _returnArg = ExpressionParserUtilities.GetOperandString(exp.Arguments[1]);
            if (exp.Arguments.Count > 2)
            {
                _sortArg = ExpressionParserUtilities.GetOperandString(exp.Arguments[2]);
                _numArgs += 2;
            }

            if (exp.Arguments.Count <= 3)
            {
                return;
            }

            var dir = ExpressionParserUtilities.GetOperandString(exp.Arguments[3]);
            if (!Enum.TryParse(dir, out SortDirection enumeratedDir))
            {
                return;
            }

            _direction = enumeratedDir;
            _numArgs++;
        }


        public override string? ResultName => $"{_returnArg?[1..]}_{Function}";


        public override IEnumerable<string> Serialize()
        {
            var ret = new List<string?>
            {
                "REDUCE",
                Function.ToString(),
                _numArgs.ToString(),
                _returnArg,
            };
            if (!string.IsNullOrEmpty(_sortArg))
            {
                ret.Add("BY");
                ret.Add($"@{_sortArg}");

                if (_direction != null)
                {
                    ret.Add(_direction == SortDirection.Ascending ? "ASC" : "DESC");
                }
            }

            ret.Add("AS");
            ret.Add(ResultName);
            return ret.ToArray()!;
        }
    }
}