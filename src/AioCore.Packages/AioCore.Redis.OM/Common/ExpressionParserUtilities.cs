using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using AioCore.Redis.OM.Aggregation;
using AioCore.Redis.OM.Aggregation.AggregationPredicates;
using AioCore.Redis.OM.Modeling;

namespace AioCore.Redis.OM.Common
{
    internal static class ExpressionParserUtilities
    {
        internal static string? GetOperandString(Expression exp)
        {
            return exp switch
            {
                ConstantExpression constExp => constExp.Value?.ToString(),
                MemberExpression member => GetOperandStringForMember(member),
                MethodCallExpression method when method.Method.Name == "get_Item" =>
                    $"@{((ConstantExpression) method.Arguments[0]).Value}",
                MethodCallExpression method => GetOperandString(method),
                UnaryExpression unary => GetOperandString(unary.Operand),
                BinaryExpression binExpression => ParseBinaryExpression(binExpression),
                LambdaExpression lambda => GetOperandString(lambda.Body),
                _ => string.Empty
            };
        }


        internal static string? GetOperandString(MethodCallExpression exp)
        {
            var mathMethods = new List<string> {"log", "abs", "ceil", "floor", "log2", "exp", "sqrt"};
            var methodName = MapMethodName(exp.Method.Name);

            return methodName switch
            {
                "split" => ParseSplitMethod(exp),
                "format" => ParseFormatMethod(exp),
                _ => mathMethods.Contains(methodName) ? ParseMathMethod(exp, methodName) : ParseMethod(exp, methodName)
            };
        }


        internal static string? GetOperandStringForQueryArgs(Expression exp)
        {
            return exp switch
            {
                ConstantExpression constExp => $"{constExp.Value}",
                MemberExpression member => GetOperandStringForMember(member),
                MethodCallExpression method => TranslateContainsStandardQuerySyntax(method),
                UnaryExpression unary => GetOperandStringForQueryArgs(unary.Operand),
                _ => throw new ArgumentException("Unrecognized Expression type")
            };
        }


        internal static object? GetValue(MemberInfo memberInfo, object? forObject)
        {
            return memberInfo.MemberType switch
            {
                MemberTypes.Field => ((FieldInfo) memberInfo).GetValue(forObject),
                MemberTypes.Property => ((PropertyInfo) memberInfo).GetValue(forObject),
                _ => throw new NotImplementedException()
            };
        }


        internal static string? ParseBinaryExpression(BinaryExpression rootBinaryExpression, bool filterFormat = false)
        {
            var operationStack = new Stack<string?>();
            var binExpressions = SplitBinaryExpression(rootBinaryExpression);
            foreach (var expression in binExpressions)
            {
                var right = GetOperandString(expression.Right);
                var left = GetOperandString(expression.Left);

                if (filterFormat && expression.Left is MemberExpression mem &&
                    mem.Type == typeof(string))
                {
                    right = $"'{right}'";
                }

                operationStack.Push(right);
                operationStack.Push(GetOperatorFromNodeType(expression.NodeType));
                if (!string.IsNullOrEmpty(left))
                {
                    operationStack.Push(left);
                }
            }

            return string.Join(" ", operationStack);
        }


        internal static string? TranslateMethodExpressions(MethodCallExpression exp)
        {
            return exp.Method.Name switch
            {
                "Contains" => TranslateContainsStandardQuerySyntax(exp),
                _ => throw new ArgumentException($"Unrecognized method for query translation:{exp.Method.Name}")
            };
        }

        internal static string GetSearchFieldNameFromMember(MemberExpression member)
        {
            var stack = GetMemberChain(member);
            var topMember = stack.Peek();
            var memberPath = stack.Select(x => x.Name).ToArray();

            if (topMember != member.Member) return string.Join("_", memberPath);
            {
                var searchField = member.Member.GetCustomAttributes().Where(x => x is SearchFieldAttribute)
                    .Cast<SearchFieldAttribute>().FirstOrDefault();
                if (searchField != null && !string.IsNullOrEmpty(searchField.PropertyName))
                {
                    return searchField.PropertyName;
                }
            }

            return string.Join("_", memberPath);
        }

        private static Stack<MemberInfo> GetMemberChain(MemberExpression memberExpression)
        {
            var memberStack = new Stack<MemberInfo>();
            memberStack.Push(memberExpression.Member);

            var parentExpression = memberExpression.Expression;
            while (parentExpression is MemberExpression parentMember)
            {
                if (parentMember.Member.Name == nameof(AggregationResult<object>.RecordShell))
                {
                    break;
                }

                memberStack.Push(parentMember.Member);
                parentExpression = parentMember.Expression;
            }

            return memberStack;
        }

        internal static SearchFieldAttribute? DetermineSearchAttribute(MemberExpression memberExpression)
        {
            var memberChain = GetMemberChain(memberExpression);
            SearchFieldAttribute? attr;
            do
            {
                var memberInfo = memberChain.Pop();
                attr = memberInfo
                    .GetCustomAttributes()
                    .Where(x => x is SearchFieldAttribute)
                    .Cast<SearchFieldAttribute>()
                    .FirstOrDefault(x => x.JsonPath?.Split('.').Last() == memberExpression.Member.Name);
            } while (attr == null && memberChain.Any());

            return attr ??= memberExpression.Member.GetCustomAttributes().Where(x => x is SearchFieldAttribute)
                .Cast<SearchFieldAttribute>().FirstOrDefault();
        }

        private static string? GetOperandStringForMember(MemberExpression member)
        {
            var memberPath = new List<string>();
            if (memberPath == null) throw new ArgumentNullException(nameof(memberPath));
            var parentExpression = member.Expression;
            while (parentExpression is MemberExpression parentMember)
            {
                memberPath.Add(parentMember.Member.Name);
                parentExpression = parentMember.Expression;
            }

            memberPath.Add(member.Member.Name);

            var searchField = member.Member.GetCustomAttributes().Where(x => x is SearchFieldAttribute)
                .Cast<SearchFieldAttribute>().FirstOrDefault();
            if (searchField == null)
            {
                if (member.Expression is not ConstantExpression c)
                {
                    try
                    {
                        return Expression.Lambda(member).Compile().DynamicInvoke()?.ToString();
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"Could not retrieve value from {member.Member.Name}, most likely, it is not properly decorated in the model defining the index.",
                            ex);
                    }
                }

                var val = GetValue(member.Member, c.Value);
                return val?.ToString();
            }

            var propertyName = GetSearchFieldNameFromMember(member);
            return $"@{propertyName}";
        }

        private static string? GetOperandStringStringArgs(Expression exp)
        {
            return exp switch
            {
                ConstantExpression constExp => constExp.Type == typeof(string)
                    ? $"\"{constExp.Value}\""
                    : $"{constExp.Value}",
                MemberExpression member => GetOperandStringForMember(member),
                MethodCallExpression method => $"@{((ConstantExpression) method.Arguments[0]).Value}",
                UnaryExpression unary => GetOperandString(unary.Operand),
                BinaryExpression binExpression => ParseBinaryExpression(binExpression),
                _ => string.Empty
            };
        }

        private static string MapMethodName(string methodName) => methodName switch
        {
            nameof(string.ToUpper) => "upper",
            nameof(string.ToLower) => "lower",
            nameof(string.Contains) => "contains",
            nameof(string.StartsWith) => "startswith",
            nameof(string.Substring) => "substr",
            nameof(string.Format) => "format",
            nameof(string.Split) => "split",
            nameof(Math.Log) => "log2",
            nameof(Math.Log10) => "log",
            nameof(Math.Ceiling) => "ceil",
            nameof(Math.Floor) => "floor",
            nameof(Math.Exp) => "exp",
            nameof(Math.Abs) => "abs",
            nameof(Math.Sqrt) => "sqrt",
            nameof(ApplyFunctions.Day) => "day",
            nameof(ApplyFunctions.Hour) => "hour",
            nameof(ApplyFunctions.Minute) => "minute",
            nameof(ApplyFunctions.Month) => "month",
            nameof(ApplyFunctions.DayOfWeek) => "dayofweek",
            nameof(ApplyFunctions.DayOfMonth) => "dayofmonth",
            nameof(ApplyFunctions.DayOfYear) => "dayofyear",
            nameof(ApplyFunctions.Year) => "year",
            nameof(ApplyFunctions.MonthOfYear) => "monthofyear",
            nameof(ApplyFunctions.FormatTimestamp) => "timefmt",
            nameof(ApplyFunctions.ParseTime) => "parsetime",
            nameof(ApplyFunctions.Exists) => "exists",
            _ => string.Empty
        };

        private static string? ParseMathMethod(MethodCallExpression exp, string methodName)
        {
            var sb = new StringBuilder();
            sb.Append($"{methodName}(");
            sb.Append(GetOperandString(exp.Arguments[0]));
            sb.Append(')');
            return sb.ToString();
        }

        private static string? ParseMethod(MethodCallExpression exp, string methodName)
        {
            var sb = new StringBuilder();
            var args = new List<string?>();
            sb.Append($"{methodName}(");
            if (exp.Object != null)
            {
                args.Add(GetOperandStringStringArgs(exp.Object));
            }

            args.AddRange(exp.Arguments.Select(GetOperandStringStringArgs));
            sb.Append(string.Join(",", args));
            if (methodName == "substr" && args.Count == 2)
            {
                sb.Append(",-1");
            }

            sb.Append(')');
            return sb.ToString();
        }

        private static string? ParseFormatMethod(MethodCallExpression exp)
        {
            const string pattern = "\\{(\\d+|)\\}";
            var args = new List<string>();
            var sb = new StringBuilder();
            sb.Append("format(");
            var formatStringExpression = exp.Arguments[0];
            var formatArgs = new List<string?>();
            var formatString = string.Empty;
            switch (formatStringExpression)
            {
                case ConstantExpression constantFormattedExpression:
                    formatString = constantFormattedExpression.Value?.ToString();
                    if (formatString != null) args.Add($"\"{Regex.Replace(formatString, pattern, "%s")}\"");
                    break;
                case MemberExpression {Expression: ConstantExpression constInnerExpression} member:
                    formatString = (string) GetValue(member.Member, constInnerExpression.Value)!;
                    args.Add($"\"{Regex.Replace(formatString, pattern, "%s")}\"");
                    break;
            }

            for (var i = 1; i < exp.Arguments.Count; i++)
            {
                formatArgs.Add(GetOperandStringStringArgs(exp.Arguments[i]));
            }

            if (formatString != null)
            {
                var matches = Regex.Matches(formatString, pattern);
                args.AddRange(from Match? match in matches
                    select match.Value.Substring(1, match.Length - 2)
                    into subStr
                    select int.Parse(subStr)
                    into matchIndex
                    select formatArgs[matchIndex]);
            }

            sb.Append(string.Join(",", args));
            sb.Append(')');
            return sb.ToString();
        }

        private static string? GetOperatorFromNodeType(ExpressionType type)
        {
            return type switch
            {
                ExpressionType.Add => "+",
                ExpressionType.Subtract => "-",
                ExpressionType.Divide => "/",
                ExpressionType.Modulo => "%",
                ExpressionType.Power => "^",
                ExpressionType.ExclusiveOr => "^",
                ExpressionType.Multiply => "*",
                ExpressionType.Equal => "==",
                ExpressionType.OrElse => "||",
                ExpressionType.AndAlso => "&&",
                ExpressionType.Not => "!",
                ExpressionType.NotEqual => "!=",
                ExpressionType.LessThan => "<",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.GreaterThan => ">",
                ExpressionType.GreaterThanOrEqual => ">=",
                _ => string.Empty
            };
        }

        private static string? ParseSplitMethod(MethodCallExpression exp)
        {
            var args = new List<string?>();
            var sb = new StringBuilder();
            sb.Append("split(");
            args.Add(GetOperandStringStringArgs(exp.Object ??
                                                throw new InvalidOperationException(
                                                    "Object within expression is null.")));
            var arg = exp.Arguments[0];
            if (arg.Type == typeof(string) || arg.Type == typeof(char))
            {
                args.Add($"\"{GetOperandStringStringArgs(arg)}\"");
            }
            else
            {
                switch (arg)
                {
                    case MemberExpression {Expression: ConstantExpression constExp} member:
                    {
                        var innerArgList = new List<string>();
                        if (member.Type == typeof(char[]))
                        {
                            var charArr = (char[]) GetValue(member.Member, constExp.Value)!;
                            innerArgList.AddRange(charArr.Select(c => c.ToString()));
                        }
                        else if (member.Type == typeof(string[]))
                        {
                            var stringArr = (string[]) GetValue(member.Member, constExp.Value)!;
                            innerArgList.AddRange(stringArr);
                        }

                        args.Add($"\"{string.Join(",", innerArgList)}\"");
                        break;
                    }

                    case NewArrayExpression arrayExpression:
                    {
                        var innerArgList = new List<string?>();
                        foreach (var item in arrayExpression.Expressions)
                        {
                            if (item is ConstantExpression constant)
                            {
                                innerArgList.Add(constant.Value?.ToString());
                            }
                        }

                        args.Add($"\"{string.Join(",", innerArgList)}\"");
                        break;
                    }
                }
            }

            sb.Append(string.Join(",", args));
            sb.Append(')');
            return sb.ToString();
        }

        private static IEnumerable<BinaryExpression> SplitBinaryExpression(BinaryExpression exp)
        {
            var list = new List<BinaryExpression>();
            do
            {
                list.Add(exp);
                switch (exp.Left)
                {
                    case UnaryExpression unExp:
                        if (unExp.Operand is BinaryExpression inner)
                        {
                            exp = inner;
                        }
                        else
                        {
                            return list;
                        }

                        break;
                    case BinaryExpression left:
                        exp = left;
                        break;
                    default:
                        return list;
                }
            } while (true);
        }

        private static string? TranslateContainsStandardQuerySyntax(MethodCallExpression exp)
        {
            MemberExpression? expression = null;
            if (exp.Object is MemberExpression expObject)
            {
                expression = expObject;
            }
            else if (exp.Arguments.FirstOrDefault() is MemberExpression)
            {
                expression = exp.Arguments.FirstOrDefault() as MemberExpression;
            }

            if (expression == null)
            {
                throw new InvalidOperationException($"Could not parse query for Contains");
            }

            var type = Nullable.GetUnderlyingType(expression.Type) ?? expression.Type;

            var memberName = GetOperandStringForMember(expression);
            var literal = GetOperandStringForQueryArgs(exp.Arguments.Last());
            return (type == typeof(string)) ? $"{memberName}:{literal}" : $"{memberName}:{{{literal}}}";
        }
    }
}