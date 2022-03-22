using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using AioCore.Redis.OM.Common;
using AioCore.Redis.OM.Modeling;

namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class QueryPredicate : BooleanExpression, IAggregationPredicate
    {
        public QueryPredicate(LambdaExpression exp)
            : base(exp)
        {
        }


        public QueryPredicate()
            : base(System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression.Constant("*")))
        {
        }


        public IEnumerable<string> Serialize()
        {
            var predicateStack = SplitExpression();
            return new[] {string.Join(" ", predicateStack)};
        }


        protected override void ValidateAndPushOperand(Expression expression, Stack<string> stack)
        {
            switch (expression)
            {
                case BinaryExpression {Left: MemberExpression memberExpression} binaryExpression:
                    switch (binaryExpression.Right)
                    {
                        case ConstantExpression constantExpression:
                            stack.Push(BuildQueryPredicate(binaryExpression.NodeType, memberExpression.Member,
                                constantExpression));
                            break;
                        case UnaryExpression uni:
                            switch (uni.Operand)
                            {
                                case ConstantExpression c:
                                    stack.Push(BuildQueryPredicate(binaryExpression.NodeType, memberExpression.Member,
                                        c));
                                    break;
                                case MemberExpression {Expression: ConstantExpression frame} mem:
                                {
                                    var val = ExpressionParserUtilities.GetValue(mem.Member, frame.Value);
                                    stack.Push(BuildQueryPredicate(binaryExpression.NodeType, memberExpression.Member,
                                        System.Linq.Expressions.Expression.Constant(val)));
                                    break;
                                }
                            }

                            break;
                        case MemberExpression {Expression: ConstantExpression frame} mem:
                        {
                            var val = ExpressionParserUtilities.GetValue(mem.Member, frame.Value);
                            stack.Push(BuildQueryPredicate(binaryExpression.NodeType, memberExpression.Member,
                                System.Linq.Expressions.Expression.Constant(val)));
                            break;
                        }
                    }

                    break;
                case ConstantExpression c when c.Value?.ToString() == "*":
                    stack.Push(c.Value.ToString()!);
                    break;
                default:
                    throw new ArgumentException("Invalid Expression Type");
            }
        }


        protected override void SplitBinaryExpression(BinaryExpression expression, Stack<string> stack)
        {
            if (expression.Left is BinaryExpression left)
            {
                SplitBinaryExpression(left, stack);
                ValidateAndPushOperand(expression.Right, stack);
            }
            else
            {
                ValidateAndPushOperand(expression, stack);
            }
        }

        private static string BuildEqualityPredicate(MemberInfo member, ConstantExpression expression)
        {
            var sb = new StringBuilder();
            var fieldAttribute = member.GetCustomAttribute<SearchFieldAttribute>();
            if (fieldAttribute == null)
            {
                throw new InvalidOperationException(
                    "Searches can only be performed on fields marked with a RedisFieldAttribute with the SearchFieldType not set to None");
            }

            sb.Append($"@{member.Name}:");
            var searchFieldType = fieldAttribute.SearchFieldType != SearchFieldType.INDEXED
                ? fieldAttribute.SearchFieldType
                : ExpressionTranslator.DetermineIndexFieldsType(member);
            switch (searchFieldType)
            {
                case SearchFieldType.TAG:
                    sb.Append($"{{{expression.Value}}}");
                    break;
                case SearchFieldType.TEXT:
                    sb.Append(expression.Value);
                    break;
                case SearchFieldType.NUMERIC:
                    sb.Append($"[{expression.Value} {expression.Value}]");
                    break;
                default:
                    throw new InvalidOperationException(
                        "Could not translate query equality searches only supported for Tag, text, and numeric fields");
            }

            return sb.ToString();
        }

        private string BuildQueryPredicate(ExpressionType expType, MemberInfo member,
            ConstantExpression constExpression)
        {
            var queryPredicate = expType switch
            {
                ExpressionType.GreaterThan => $"@{member.Name}:[({constExpression.Value} inf]",
                ExpressionType.LessThan => $"@{member.Name}:[-inf ({constExpression.Value}]",
                ExpressionType.GreaterThanOrEqual => $"@{member.Name}:[{constExpression.Value} inf]",
                ExpressionType.LessThanOrEqual => $"@{member.Name}:[-inf {constExpression.Value}]",
                ExpressionType.Equal => BuildEqualityPredicate(member, constExpression),
                ExpressionType.NotEqual => $"@{member.Name} : -{{{constExpression.Value}}}",
                _ => string.Empty
            };
            return queryPredicate;
        }
    }
}