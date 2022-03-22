using System.Linq.Expressions;
using AioCore.Redis.OM.Common;

namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class FilterPredicate : Apply, IAggregationPredicate
    {
        public FilterPredicate(Expression expression)
            : base(expression, string.Empty)
        {
        }


        public new IEnumerable<string> Serialize()
        {
            var list = new List<string?> {"FILTER"};
            switch (Expression)
            {
                case BinaryExpression rootBinExpression:
                    list.Add(ExpressionParserUtilities.ParseBinaryExpression(rootBinExpression, true));
                    break;
                case MethodCallExpression method:
                    list.Add(ExpressionParserUtilities.GetOperandString(method));
                    break;
                default:
                    list.Add(ExpressionParserUtilities.GetOperandString(Expression));
                    break;
            }

            return list.ToArray()!;
        }
    }
}