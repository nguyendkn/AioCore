using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using AioCore.Redis.OM.Aggregation;
using AioCore.Redis.OM.Aggregation.AggregationPredicates;
using AioCore.Redis.OM.Modeling;
using AioCore.Redis.OM.Searching;
using AioCore.Redis.OM.Searching.Query;

namespace AioCore.Redis.OM.Common
{
    internal class ExpressionTranslator
    {
        private static readonly char[] TagEscapeChars =
        {
            ',', '.', '<', '>', '{', '}', '[', ']', '"', '\'', ':', ';',
            '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '~', '|', ' ',
        };


        public static RedisAggregation BuildAggregationFromExpression(Expression expression, Type type)
        {
            var attr = type.GetCustomAttribute<DocumentAttribute>();
            if (attr == null)
            {
                throw new InvalidOperationException(
                    "Aggregations can only be performed on objects decorated with a RedisObjectDefinitionAttribute that specifies a particular index");
            }

            var indexName = string.IsNullOrEmpty(attr.IndexName) ? $"{type.Name.ToLower()}-idx" : attr.IndexName;
            var aggregation = new RedisAggregation(indexName!);
            if (expression is not MethodCallExpression methodExpression)
            {
                return aggregation;
            }

            var expressions = new List<MethodCallExpression> {methodExpression};
            while (methodExpression.Arguments[0] is MethodCallExpression innerExpression)
            {
                expressions.Add(innerExpression);
                methodExpression = innerExpression;
            }

            for (var i = 0; i < expressions.Count; i++)
            {
                var exp = expressions[i];
                LambdaExpression lambda;
                switch (exp.Method.Name)
                {
                    case "FirstOrDefault":
                    case "First":
                    case "FirstOrDefaultAsync":
                    case "FirstAsync":
                        aggregation.Limit = new LimitPredicate {Count = 1};
                        break;
                    case "Where":
                        lambda = (LambdaExpression) ((UnaryExpression) exp.Arguments[1]).Operand;
                        if (i == expressions.Count - 1)
                        {
                            aggregation.Query = new QueryPredicate(lambda);
                        }
                        else
                        {
                            aggregation.Predicates.Push(new FilterPredicate(lambda.Body));
                        }

                        break;
                    case "Average":
                    case "AverageAsync":
                        TranslateAndPushReductionPredicate(exp, ReduceFunction.AVG, aggregation.Predicates);
                        break;
                    case "StandardDeviation":
                    case "StandardDeviationAsync":
                        TranslateAndPushReductionPredicate(exp, ReduceFunction.STDDEV, aggregation.Predicates);
                        break;
                    case "Sum":
                    case "SumAsync":
                        TranslateAndPushReductionPredicate(exp, ReduceFunction.SUM, aggregation.Predicates);
                        break;
                    case "Min":
                    case "MinAsync":
                        TranslateAndPushReductionPredicate(exp, ReduceFunction.MIN, aggregation.Predicates);
                        break;
                    case "Max":
                    case "MaxAsync":
                        TranslateAndPushReductionPredicate(exp, ReduceFunction.MAX, aggregation.Predicates);
                        break;
                    case "OrderBy":
                        aggregation.Predicates.Push(TranslateSortBy(exp, SortDirection.Ascending));
                        break;
                    case "OrderByDescending":
                        aggregation.Predicates.Push(TranslateSortBy(exp, SortDirection.Descending));
                        break;
                    case "Take":
                        if (aggregation.Limit != null)
                        {
                            aggregation.Limit.Count = TranslateTake(exp);
                        }
                        else
                        {
                            aggregation.Limit = new LimitPredicate {Count = TranslateTake(exp)};
                        }

                        break;
                    case "Skip":
                        if (aggregation.Limit != null)
                        {
                            aggregation.Limit.Count = TranslateSkip(exp);
                        }
                        else
                        {
                            aggregation.Limit = new LimitPredicate {Offset = TranslateSkip(exp)};
                        }

                        break;
                    case "Count":
                    case "LongCount":
                    case "CountAsync":
                    case "LongCountAsync":
                        TranslateAndPushZeroArgumentPredicate(ReduceFunction.COUNT, aggregation.Predicates);
                        break;
                    case "CountDistinct":
                    case "CountDistinctAsync":
                        TranslateAndPushReductionPredicate(exp, ReduceFunction.COUNT_DISTINCT, aggregation.Predicates);
                        break;
                    case "CountDistinctTish":
                    case "CountDistinctTishAsync":
                        TranslateAndPushReductionPredicate(exp, ReduceFunction.COUNT_DISTINCTISH,
                            aggregation.Predicates);
                        break;
                    case "GroupBy":
                        TranslateAndPushGroupBy(aggregation.Predicates, exp);
                        break;
                    case "Apply":
                        aggregation.Predicates.Push(TranslateApplyPredicate(exp));
                        break;
                    case "Filter":
                        lambda = (LambdaExpression) ((UnaryExpression) exp.Arguments[1]).Operand;
                        aggregation.Predicates.Push(new FilterPredicate(lambda.Body));
                        break;
                    case "Quantile":
                    case "QuantileAsync":
                        TranslateAndPushTwoArgumentReductionPredicate(exp, ReduceFunction.QUANTILE,
                            aggregation.Predicates);
                        break;
                    case "Distinct":
                    case "DistinctAsync":
                        TranslateAndPushReductionPredicate(exp, ReduceFunction.TOLIST, aggregation.Predicates);
                        break;
                    case "FirstValue":
                    case "FirstValueAsync":
                        TranslateAndPushFirstValuePredicate(exp, aggregation.Predicates);
                        break;
                    case "RandomSample":
                    case "RandomSampleAsync":
                        TranslateAndPushTwoArgumentReductionPredicate(exp, ReduceFunction.RANDOM_SAMPLE,
                            aggregation.Predicates);
                        break;
                }
            }

            return aggregation;
        }


        internal static RedisQuery BuildQueryFromExpression(Expression expression, Type type)
        {
            var attr = type.GetCustomAttribute<DocumentAttribute>();
            if (attr == null)
            {
                throw new InvalidOperationException(
                    "Searches can only be performed on objects decorated with a RedisObjectDefinitionAttribute that specifies a particular index");
            }

            var indexName = string.IsNullOrEmpty(attr.IndexName) ? $"{type.Name.ToLower()}-idx" : attr.IndexName;
            var query = new RedisQuery(indexName!) {QueryText = "*"};
            switch (expression)
            {
                case MethodCallExpression methodExpression:
                {
                    var expressions = new List<MethodCallExpression> {methodExpression};
                    while (methodExpression.Arguments[0] is MethodCallExpression innerExpression)
                    {
                        expressions.Add(innerExpression);
                        methodExpression = innerExpression;
                    }

                    foreach (var exp in expressions)
                    {
                        switch (exp.Method.Name)
                        {
                            case "Where":
                                query.QueryText = TranslateWhereMethod(exp);
                                break;
                            case "OrderBy":
                                query.SortBy = TranslateOrderByMethod(exp, true);
                                break;
                            case "OrderByDescending":
                                query.SortBy = TranslateOrderByMethod(exp, false);
                                break;
                            case "Select":
                                query.Return = TranslateSelectMethod(exp);
                                break;
                            case "Take":
                                query.Limit ??= new SearchLimit {Offset = 0};
                                query.Limit.Number = TranslateTake(exp);
                                break;
                            case "Skip":
                                query.Limit ??= new SearchLimit {Number = 100};
                                query.Limit.Offset = TranslateSkip(exp);
                                break;
                            case "First":
                            case "Any":
                            case "FirstOrDefault":
                                query.Limit ??= new SearchLimit {Offset = 0};
                                query.Limit.Number = 1;
                                if (exp.Arguments.Count > 1)
                                {
                                    query.QueryText = TranslateFirstMethod(exp);
                                }

                                break;
                        }
                    }

                    break;
                }

                case LambdaExpression lambda:
                    query.QueryText = BuildQueryFromExpression(lambda.Body);
                    break;
            }

            return query;
        }


        internal static SearchFieldType DetermineIndexFieldsType(MemberInfo member)
        {
            if (member is PropertyInfo info && TypeDeterminationUtilities.IsNumeric(info.PropertyType))
            {
                return SearchFieldType.NUMERIC;
            }

            return SearchFieldType.TAG;
        }


        private static string GetFieldName(Expression exp)
        {
            return exp switch
            {
                ConstantExpression constExp => constExp.Value?.ToString()!,
                MemberExpression member => $"{member.Member.Name}",
                MethodCallExpression method => $"{((ConstantExpression) method.Arguments[0]).Value}",
                UnaryExpression unary => GetFieldName(unary.Operand),
                LambdaExpression lambda => GetFieldName(lambda.Body),
                _ => throw new ArgumentException("Invalid expression type detected when parsing Field Name")
            };
        }


        private static string[] GetFieldNamesGroupBy(Expression exp)
        {
            return (exp switch
            {
                ConstantExpression constExp => new[] {constExp.Value!.ToString()},
                MemberExpression member => new[] {$"{member.Member.Name}"},
                MethodCallExpression method => new[] {$"{((ConstantExpression) method.Arguments[0]).Value}"},
                UnaryExpression unary => GetFieldNamesGroupBy(unary.Operand),
                LambdaExpression lambda => GetFieldNamesGroupBy(lambda.Body),
                NewExpression newExpression => newExpression.Members?.Select(x => $"{x.Name}").ToArray(),
                _ => throw new ArgumentException("Invalid expression type detected")
            } ?? default!)!;
        }


        private static void TranslateAndPushGroupBy(Stack<IAggregationPredicate> predicates,
            MethodCallExpression expression)
        {
            var properties = GetFieldNamesGroupBy(expression.Arguments[1]);
            if (predicates.Count > 0 && predicates.Peek() is GroupBy)
            {
                var gb = (GroupBy) predicates.Pop();
                var p = new List<string>();
                p.AddRange(properties);
                p.AddRange(gb.Properties);
                gb.Properties = p.ToArray();
                predicates.Push(gb);
            }
            else
            {
                predicates.Push(new GroupBy(properties));
            }
        }

        private static bool CheckForGroupBy(Stack<IAggregationPredicate> predicates)
        {
            return predicates.Count == 0 ||
                   (predicates.Peek() is not GroupBy && predicates.Peek() is not SingleArgumentReduction);
        }

        private static bool CheckMoveGroupBy(Stack<IAggregationPredicate> predicates)
        {
            return predicates.Count > 0 && predicates.Peek() is GroupBy;
        }

        private static IAggregationPredicate TranslateApplyPredicate(MethodCallExpression exp)
        {
            var alias = ((ConstantExpression) exp.Arguments[2]).Value?.ToString();
            var lambda = (LambdaExpression) ((UnaryExpression) exp.Arguments[1]).Operand;
            return new Apply(lambda.Body, alias);
        }

        private static AggregateSortBy TranslateSortBy(MethodCallExpression expression, SortDirection dir)
        {
            var member = GetFieldName(expression.Arguments[1]);
            var sb = new AggregateSortBy(member, dir);
            return sb;
        }

        private static void PushReduction(IAggregationPredicate reduction, Stack<IAggregationPredicate> operationStack)
        {
            var pushGroupBy = CheckForGroupBy(operationStack);
            var moveGroupBy = CheckMoveGroupBy(operationStack);
            if (moveGroupBy)
            {
                var gb = operationStack.Pop();
                operationStack.Push(reduction);
                operationStack.Push(gb);
            }
            else
            {
                operationStack.Push(reduction);
                if (pushGroupBy)
                {
                    operationStack.Push(new GroupBy(Array.Empty<string>()));
                }
            }
        }

        private static void TranslateAndPushZeroArgumentPredicate(ReduceFunction function,
            Stack<IAggregationPredicate> stack)
        {
            var reduction = new ZeroArgumentReduction(function);
            PushReduction(reduction, stack);
        }

        private static void TranslateAndPushReductionPredicate(MethodCallExpression expression, ReduceFunction function,
            Stack<IAggregationPredicate> stack)
        {
            var member = GetFieldName(expression.Arguments[1]);
            var reduction = new SingleArgumentReduction(function, member);
            PushReduction(reduction, stack);
        }

        private static void TranslateAndPushFirstValuePredicate(MethodCallExpression expression,
            Stack<IAggregationPredicate> stack)
        {
            var reduction = new FirstValueReduction(expression);
            PushReduction(reduction, stack);
        }

        private static void TranslateAndPushTwoArgumentReductionPredicate(MethodCallExpression expression,
            ReduceFunction function, Stack<IAggregationPredicate> stack)
        {
            var reduction = new TwoArgumentReduction(function, expression);
            PushReduction(reduction, stack);
        }

        private static int TranslateTake(MethodCallExpression exp) =>
            (int) (((ConstantExpression) exp.Arguments[1]).Value ?? 0);

        private static int TranslateSkip(MethodCallExpression exp) =>
            (int) (((ConstantExpression) exp.Arguments[1]).Value ?? 0);

        private static ReturnFields TranslateSelectMethod(MethodCallExpression expression)
        {
            var predicate = (UnaryExpression) expression.Arguments[1];
            var lambda = (LambdaExpression) predicate.Operand;

            if (lambda.Body is MemberExpression member)
            {
                var properties = new[] {member.Member.Name};
                return new ReturnFields(properties);
            }
            else
            {
                var properties = lambda.ReturnType.GetProperties().Select(x => x.Name);
                return new ReturnFields(properties);
            }
        }

        private static RedisSortBy TranslateOrderByMethod(MethodCallExpression expression, bool ascending)
        {
            var sb = new RedisSortBy();
            var predicate = (UnaryExpression) expression.Arguments[1];
            var lambda = (LambdaExpression) predicate.Operand;
            var memberExpression = (MemberExpression) lambda.Body;
            sb.Field = memberExpression.Member.Name;
            sb.Direction = ascending ? SortDirection.Ascending : SortDirection.Descending;
            return sb;
        }

        private static string? BuildQueryFromExpression(Expression exp)
        {
            switch (exp)
            {
                case BinaryExpression binExp:
                    return TranslateBinaryExpression(binExp);
                case MethodCallExpression method:
                    return ExpressionParserUtilities.TranslateMethodExpressions(method);
                case UnaryExpression uni:
                {
                    var operandString = BuildQueryFromExpression(uni.Operand);
                    if (uni.NodeType == ExpressionType.Not)
                    {
                        operandString = $"-{operandString}";
                    }

                    return operandString;
                }
                default:
                    throw new ArgumentException("Un Parseable Lambda Body detected");
            }
        }

        private static string TranslateBinaryExpression(BinaryExpression binExpression)
        {
            var sb = new StringBuilder();
            switch (binExpression.Left)
            {
                case BinaryExpression leftBin when binExpression.Right is BinaryExpression rightBin:
                    sb.Append('(');
                    sb.Append(TranslateBinaryExpression(leftBin));
                    sb.Append(SplitPredicateSeparators(binExpression.NodeType));
                    sb.Append(TranslateBinaryExpression(rightBin));
                    sb.Append(')');
                    break;
                case BinaryExpression left:
                    sb.Append('(');
                    sb.Append(TranslateBinaryExpression(left));
                    sb.Append(SplitPredicateSeparators(binExpression.NodeType));
                    sb.Append(ExpressionParserUtilities.GetOperandStringForQueryArgs(binExpression.Right));
                    sb.Append(')');
                    break;
                default:
                {
                    if (binExpression.Right is BinaryExpression right)
                    {
                        sb.Append('(');
                        sb.Append(ExpressionParserUtilities.GetOperandStringForQueryArgs(binExpression.Left));
                        sb.Append(SplitPredicateSeparators(binExpression.NodeType));
                        sb.Append(TranslateBinaryExpression(right));
                        sb.Append(')');
                    }
                    else
                    {
                        var leftContent = ExpressionParserUtilities.GetOperandStringForQueryArgs(binExpression.Left);

                        var rightContent = ExpressionParserUtilities.GetOperandStringForQueryArgs(binExpression.Right);

                        if (binExpression.Left is MemberExpression member)
                        {
                            var predicate = BuildQueryPredicate(binExpression.NodeType, leftContent, rightContent,
                                member);
                            sb.Append('(');
                            sb.Append(predicate);
                            sb.Append(')');
                        }
                        else
                        {
                            throw new ArgumentException("Left side of expression must be a member of the search class");
                        }
                    }

                    break;
                }
            }

            return sb.ToString();
        }

        private static string? TranslateFirstMethod(MethodCallExpression expression)
        {
            var predicate = (UnaryExpression) expression.Arguments[1];
            var lambda = (LambdaExpression) predicate.Operand;
            return BuildQueryFromExpression(lambda.Body);
        }

        private static string? TranslateWhereMethod(MethodCallExpression expression)
        {
            var predicate = (UnaryExpression) expression.Arguments[1];
            var lambda = (LambdaExpression) predicate.Operand;
            return BuildQueryFromExpression(lambda.Body);
        }

        private static string BuildQueryPredicate(ExpressionType expType, string? left, string? right,
            MemberExpression memberExpression)
        {
            var queryPredicate = expType switch
            {
                ExpressionType.GreaterThan => $"{left}:[({right} inf]",
                ExpressionType.LessThan => $"{left}:[-inf ({right}]",
                ExpressionType.GreaterThanOrEqual => $"{left}:[{right} inf]",
                ExpressionType.LessThanOrEqual => $"{left}:[-inf {right}]",
                ExpressionType.Equal => BuildEqualityPredicate(memberExpression, right),
                ExpressionType.NotEqual => BuildEqualityPredicate(memberExpression, right, true),
                _ => string.Empty
            };
            return queryPredicate;
        }

        private static string BuildEqualityPredicate(MemberExpression member, string? right, bool negated = false)
        {
            var sb = new StringBuilder();
            var fieldAttribute = ExpressionParserUtilities.DetermineSearchAttribute(member);
            if (fieldAttribute == null)
            {
                throw new InvalidOperationException("Searches can only be performed on fields marked with a " +
                                                    "RedisFieldAttribute with the SearchFieldType not set to None");
            }

            if (negated)
            {
                sb.Append('-');
            }

            sb.Append($"@{ExpressionParserUtilities.GetSearchFieldNameFromMember(member)}:");
            var searchFieldType = fieldAttribute.SearchFieldType != SearchFieldType.INDEXED
                ? fieldAttribute.SearchFieldType
                : DetermineIndexFieldsType(member.Member);
            switch (searchFieldType)
            {
                case SearchFieldType.TAG:
                    sb.Append($"{{{EscapeTagField(right)}}}");
                    break;
                case SearchFieldType.TEXT:
                    sb.Append($"\"{right}\"");
                    break;
                case SearchFieldType.NUMERIC:
                    sb.Append($"[{right} {right}]");
                    break;
                case SearchFieldType.GEO:
                case SearchFieldType.INDEXED:
                default:
                    throw new InvalidOperationException(
                        "Could not translate query, equality searches only supported for Tag and numeric fields");
            }

            return sb.ToString();
        }

        private static string EscapeTagField(string? text)
        {
            var sb = new StringBuilder();
            foreach (var c in text!)
            {
                if (TagEscapeChars.Contains(c))
                {
                    sb.Append('\\');
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        private static string SplitPredicateSeparators(ExpressionType type) => type switch
        {
            ExpressionType.OrElse => " | ",
            ExpressionType.AndAlso => " ",
            _ => throw new ArgumentException("Unknown separator type")
        };
    }
}