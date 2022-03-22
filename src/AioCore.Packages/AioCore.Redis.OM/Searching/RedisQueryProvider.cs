using System.Linq.Expressions;
using System.Reflection;
using AioCore.Redis.OM.Aggregation;
using AioCore.Redis.OM.Aggregation.AggregationPredicates;
using AioCore.Redis.OM.Common;
using AioCore.Redis.OM.Contracts;
using AioCore.Redis.OM.Modeling;

#pragma warning disable CS8633

namespace AioCore.Redis.OM.Searching
{
    internal class RedisQueryProvider : IQueryProvider
    {
        private readonly int _chunkSize;

        internal RedisQueryProvider(IRedisConnection connection, RedisCollectionStateManager stateManager,
            DocumentAttribute documentAttribute, int chunkSize)
        {
            Connection = connection;
            StateManager = stateManager;
            DocumentAttribute = documentAttribute;
            _chunkSize = chunkSize;
        }

        internal RedisQueryProvider(IRedisConnection connection, DocumentAttribute documentAttribute, int chunkSize)
        {
            Connection = connection;
            DocumentAttribute = documentAttribute;
            StateManager = new RedisCollectionStateManager(DocumentAttribute);
            _chunkSize = chunkSize;
        }


        internal IRedisConnection Connection { get; }


        internal RedisCollectionStateManager StateManager { get; set; }


        internal DocumentAttribute DocumentAttribute { get; }


        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = expression.Type;
            try
            {
                return
                    (IQueryable) Activator.CreateInstance(
                        typeof(RedisCollection<>).MakeGenericType(elementType),
                        this,
                        expression)!;
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException == null)
                {
                    throw;
                }

                throw e.InnerException;
            }
        }


        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            where TElement : notnull
        {
            return new RedisCollection<TElement>(this, expression, StateManager);
        }


        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }


        public SearchResponse<T> ExecuteQuery<T>(Expression expression)
            where T : notnull
        {
            var type = typeof(T);
            var attr = type.GetCustomAttribute<DocumentAttribute>();
            if (attr == null)
            {
                type = GetRootType((MethodCallExpression) expression);
                attr = type.GetCustomAttribute<DocumentAttribute>();
            }

            if (attr == null)
            {
                throw new InvalidOperationException(
                    "Searches can only be performed on objects decorated with a RedisObjectDefinitionAttribute that specifies a particular index");
            }

            var query = ExpressionTranslator.BuildQueryFromExpression(expression, type);
            var response = Connection.SearchRawResult(query);
            return new SearchResponse<T>(response);
        }


        public IEnumerable<AggregationResult<T>> ExecuteAggregation<T>(MethodCallExpression expression,
            Type underpinningType)
        {
            var aggregation = ExpressionTranslator.BuildAggregationFromExpression(expression, underpinningType);
            var res = Connection.Execute("FT.AGGREGATE", aggregation.Serialize());
            return AggregationResult<T>.FromRedisResult(res);
        }


        public async Task<IEnumerable<AggregationResult<T>>> ExecuteAggregationAsync<T>(MethodCallExpression expression,
            Type underpinningType)
        {
            var aggregation = ExpressionTranslator.BuildAggregationFromExpression(expression, underpinningType);
            var res = await Connection.ExecuteAsync("FT.AGGREGATE", aggregation.Serialize());
            return AggregationResult<T>.FromRedisResult(res);
        }


        public RedisReply ExecuteReductiveAggregation(MethodCallExpression expression, Type underpinningType)
        {
            var aggregation = ExpressionTranslator.BuildAggregationFromExpression(expression, underpinningType);
            var reply = Connection.Execute("FT.AGGREGATE", aggregation.Serialize());
            var res = AggregationResult.FromRedisResult(reply).ToList();
            var reductionName = ((Reduction) aggregation.Predicates.Last()).ResultName;
            if (res.Any())
            {
                return res.First()[reductionName];
            }

            if (reductionName == "COUNT")
            {
                return reply.ToArray().First();
            }

            throw new Exception("Invalid value returned by server");
        }


        public async ValueTask<RedisReply> ExecuteReductiveAggregationAsync(MethodCallExpression expression,
            Type underpinningType)
        {
            var aggregation = ExpressionTranslator.BuildAggregationFromExpression(expression, underpinningType);
            var res = AggregationResult.FromRedisResult(
                await Connection.ExecuteAsync("FT.AGGREGATE", aggregation.Serialize()));
            var reductionName = ((Reduction) aggregation.Predicates.Last()).ResultName;
            return res.First()[reductionName];
        }


        public static Type GetRootType(MethodCallExpression expression)
        {
            while (expression.Arguments[0] is MethodCallExpression innerExpression)
            {
                expression = innerExpression;
            }

            return expression.Arguments[0].Type.GenericTypeArguments[0];
        }


        public TResult Execute<TResult>(Expression expression)
            where TResult : notnull
        {
            if (expression is not MethodCallExpression methodCall)
            {
                throw new NotImplementedException();
            }

            switch (methodCall.Method.Name)
            {
                case "FirstOrDefault":
                    return FirstOrDefault<TResult>(expression)!;
                case "First":
                    return First<TResult>(expression)!;
                case "Sum":
                case "Min":
                case "Max":
                case "Average":
                case "Count":
                case "LongCount":
                    var elementType = GetRootType(methodCall);
                    var res = ExecuteReductiveAggregation(methodCall, elementType);
                    return (TResult) Convert.ChangeType(res, typeof(TResult));
            }

            throw new NotImplementedException();
        }

        private TResult First<TResult>(Expression expression)
            where TResult : notnull
        {
            var (key, value) = ExecuteQuery<TResult>(expression).Documents.First();
            StateManager.InsertIntoData(key, value);
            StateManager.InsertIntoSnapshot(key, value);
            return value;
        }

        private TResult? FirstOrDefault<TResult>(Expression expression)
            where TResult : notnull
        {
            var res = ExecuteQuery<TResult>(expression);
            if (!res.Documents.Any()) return default;
            var (key, value) = res.Documents.FirstOrDefault();
            StateManager.InsertIntoSnapshot(key, value);
            StateManager.InsertIntoData(key, value);
            return value;
        }
    }
}