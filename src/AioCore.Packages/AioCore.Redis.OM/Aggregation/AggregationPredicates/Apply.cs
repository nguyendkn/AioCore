using System.Linq.Expressions;
using AioCore.Redis.OM.Common;

namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class Apply : IAggregationPredicate
    {
        public Apply(Expression expression, string? alias)
        {
            Expression = expression;
            Alias = alias;
        }


        public string? Alias { get; set; }


        protected Expression Expression { get; }


        public IEnumerable<string> Serialize()
        {
            var list = new List<string?> {"APPLY"};
            switch (Expression)
            {
                case BinaryExpression rootBinExpression:
                    list.Add(ExpressionParserUtilities.ParseBinaryExpression(rootBinExpression));
                    break;
                case MethodCallExpression method:
                    list.Add(ExpressionParserUtilities.GetOperandString(method));
                    break;
                default:
                    list.Add(ExpressionParserUtilities.GetOperandString(Expression));
                    break;
            }

            list.Add("AS");
            list.Add(Alias);
            return list.ToArray()!;
        }
    }
}