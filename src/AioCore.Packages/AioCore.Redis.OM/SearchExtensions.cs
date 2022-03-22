using System.Linq.Expressions;
using System.Reflection;
using AioCore.Redis.OM.Aggregation;
using AioCore.Redis.OM.Searching;

namespace AioCore.Redis.OM
{
    public static class SearchExtensions
    {
        public static RedisAggregationSet<T> Apply<T, TR>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TR>> expression, string alias)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Apply, source, expression, alias),
                new[] {source.Expression, Expression.Quote(expression), Expression.Constant(alias)});
            return new RedisAggregationSet<T>(source, exp);
        }


        public static RedisAggregationSet<T> Filter<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, bool>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Filter, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new RedisAggregationSet<T>(source, exp);
        }


        public static int Count<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, bool>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Count, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregation(exp, typeof(T));
        }


        public static RedisAggregationSet<T> Where<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, bool>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Where, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new RedisAggregationSet<T>(source, exp);
        }


        public static IRedisCollection<T> Where<T>(this IRedisCollection<T> source,
            Expression<Func<T, bool>> expression)
            where T : notnull
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Where, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new RedisCollection<T>((RedisQueryProvider) source.Provider, exp, source.StateManager,
                source.ChunkSize);
        }


        public static IRedisCollection<TR> Select<T, TR>(this IRedisCollection<T> source,
            Expression<Func<T, TR>> expression)
            where T : notnull
            where TR : notnull
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Select, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new RedisCollection<TR>((RedisQueryProvider) source.Provider, exp, source.StateManager,
                source.ChunkSize);
        }


        public static IRedisCollection<T> Skip<T>(this IRedisCollection<T> source, int count)
            where T : notnull
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Skip, source, count),
                new[] {source.Expression, Expression.Constant(count)});
            return new RedisCollection<T>((RedisQueryProvider) source.Provider, exp, source.StateManager,
                source.ChunkSize);
        }


        public static IRedisCollection<T> Take<T>(this IRedisCollection<T> source, int count)
            where T : notnull
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Take, source, count),
                new[] {source.Expression, Expression.Constant(count)});
            return new RedisCollection<T>((RedisQueryProvider) source.Provider, exp, source.StateManager,
                source.ChunkSize);
        }


        public static long CountDistinct<T, TCount>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TCount>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(CountDistinct, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregation(exp, typeof(T));
        }


        public static async ValueTask<long> CountDistinctAsync<T, TCount>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TCount>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(CountDistinctAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static GroupedAggregationSet<T> CountDistinct<T, TCount>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TCount>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(CountDistinct, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> CountDistinctish<T, TCount>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TCount>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(CountDistinctish, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static double StandardDeviation<T, TReduce>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TReduce>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(StandardDeviation, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregation(exp, typeof(T));
        }


        public static async ValueTask<double> StandardDeviationAsync<T, TReduce>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TReduce>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(StandardDeviationAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static RedisAggregationSet<T> Distinct<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Distinct, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new RedisAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> Distinct<T, TResult>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Distinct, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static RedisReply FirstValue<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstValue, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregation(exp, typeof(T));
        }


        public static async ValueTask<RedisReply> FirstValueAsync<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstValue, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static GroupedAggregationSet<T> FirstValue<T, TResult>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstValue, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});

            return new GroupedAggregationSet<T>(source, exp);
        }


        public static RedisReply FirstValue<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression, string sortedBy, SortDirection direction)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstValue, source, expression, sortedBy, direction),
                source.Expression,
                Expression.Quote(expression),
                Expression.Constant(sortedBy),
                Expression.Constant(direction));
            return ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregation(exp, typeof(T));
        }


        public static RedisReply FirstValue<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression, string sortedBy)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstValue, source, expression, sortedBy),
                new[] {source.Expression, Expression.Quote(expression), Expression.Constant(sortedBy)});
            return ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregation(exp, typeof(T));
        }


        public static async ValueTask<RedisReply> FirstValueAsync<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression, string sortedBy, SortDirection direction)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstValueAsync, source, expression, sortedBy, direction),
                source.Expression,
                Expression.Quote(expression),
                Expression.Constant(sortedBy),
                Expression.Constant(direction));
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<RedisReply> FirstValueAsync<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression, string sortedBy)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstValueAsync, source, expression, sortedBy),
                new[] {source.Expression, Expression.Quote(expression), Expression.Constant(sortedBy)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static RedisAggregationSet<T> RandomSample<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression, long sampleSize)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(RandomSample, source, expression, sampleSize),
                new[] {source.Expression, Expression.Quote(expression), Expression.Constant(sampleSize)});
            return new RedisAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> RandomSample<T, TResult>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression, long sampleSize)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(RandomSample, source, expression, sampleSize),
                new[] {source.Expression, Expression.Quote(expression), Expression.Constant(sampleSize)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static double Quantile<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression, double quantile)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Quantile, source, expression, quantile),
                new[] {source.Expression, Expression.Quote(expression), Expression.Constant(quantile)});
            return ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregation(exp, typeof(T));
        }


        public static async ValueTask<double> QuantileAsync<T, TResult>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TResult>> expression, double quantile)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Quantile, source, expression, quantile),
                new[] {source.Expression, Expression.Quote(expression), Expression.Constant(quantile)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static GroupedAggregationSet<T> Quantile<T, TField>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression, double quantile)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Quantile, source, expression, quantile),
                new[] {source.Expression, Expression.Quote(expression), Expression.Constant(quantile)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static long CountDistinctish<T, TField>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(CountDistinctish, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregation(exp, typeof(T));
        }


        public static async ValueTask<long> CountDistinctishAsync<T, TField>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(CountDistinctish, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static GroupedAggregationSet<T> GroupBy<T, TGroupType>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TGroupType>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(GroupBy, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> GroupBy<T, TGroupType>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TGroupType>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(GroupBy, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> Average<T, TField>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Average, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> Sum<T, TField>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Sum, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> Min<T, TField>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Min, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> Max<T, TField>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Max, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> StandardDeviation<T, TField>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(StandardDeviation, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> OrderBy<T, TField>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(OrderBy, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> OrderByDescending<T, TField>(this GroupedAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(OrderByDescending, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static RedisAggregationSet<T> OrderBy<T, TField>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(OrderBy, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new RedisAggregationSet<T>(source, exp);
        }


        public static RedisAggregationSet<T> OrderByDescending<T, TField>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, TField>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(OrderByDescending, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return new RedisAggregationSet<T>(source, exp);
        }


        public static RedisAggregationSet<T> CloseGroup<T>(this GroupedAggregationSet<T> source)
        {
            return new RedisAggregationSet<T>(source, source.Expression);
        }


        public static AggregationResult<T>? FirstOrDefault<T>(this RedisAggregationSet<T> source)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstOrDefault, source),
                source.Expression);
            return ((RedisQueryProvider) source.Provider).ExecuteAggregation<T>(exp, typeof(T)).FirstOrDefault();
        }


        public static async ValueTask<AggregationResult<T>?> FirstOrDefaultAsync<T>(this RedisAggregationSet<T> source)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstOrDefault, source),
                source.Expression);
            return (await ((RedisQueryProvider) source.Provider).ExecuteAggregationAsync<T>(exp, typeof(T)))
                .FirstOrDefault();
        }


        public static async ValueTask<AggregationResult<T>> FirstAsync<T>(this RedisAggregationSet<T> source)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(FirstOrDefault, source),
                source.Expression);
            return (await ((RedisQueryProvider) source.Provider).ExecuteAggregationAsync<T>(exp, typeof(T))).First();
        }


        public static AggregationResult<T> First<T>(this RedisAggregationSet<T> source)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(First, source),
                source.Expression);
            return ((RedisQueryProvider) source.Provider).ExecuteAggregation<T>(exp, typeof(T)).First();
        }


        public static GroupedAggregationSet<T> Skip<T>(this GroupedAggregationSet<T> source, int count)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Skip, source, count),
                new[] {source.Expression, Expression.Constant(count)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> Take<T>(this GroupedAggregationSet<T> source, int count)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Take, source, count),
                new[] {source.Expression, Expression.Constant(count)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> Skip<T>(this RedisAggregationSet<T> source, int count)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Skip, source, count),
                new[] {source.Expression, Expression.Constant(count)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static GroupedAggregationSet<T> Take<T>(this RedisAggregationSet<T> source, int count)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(Take, source, count),
                new[] {source.Expression, Expression.Constant(count)});
            return new GroupedAggregationSet<T>(source, exp);
        }


        public static async ValueTask<double> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, double>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<int> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, int>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<long> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, long>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<float> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, float>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<decimal> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, decimal>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<double?> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, double?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<int?> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, int?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<long?> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, long?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<float?> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, float?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<decimal?> SumAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, decimal?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(SumAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<double> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, double>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<double> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, int>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<double> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, long>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<float> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, float>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<decimal> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, decimal>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<double?> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, double?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<int?> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, int?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<long?> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, long?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<float?> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, float?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<decimal?> AverageAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, decimal?>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(AverageAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<RedisReply> MaxAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, RedisReply>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(MaxAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }


        public static async ValueTask<RedisReply> MinAsync<T>(this RedisAggregationSet<T> source,
            Expression<Func<AggregationResult<T>, RedisReply>> expression)
        {
            var exp = Expression.Call(
                null,
                GetMethodInfo(MinAsync, source, expression),
                new[] {source.Expression, Expression.Quote(expression)});
            return await ((RedisQueryProvider) source.Provider).ExecuteReductiveAggregationAsync(exp, typeof(T));
        }

        private static MethodInfo GetMethodInfo<T1, T2>(Func<T1, T2> f)
        {
            return f.Method;
        }

        private static MethodInfo GetMethodInfo<T1, T2>(Func<T1, T2> f, T1 unused)
        {
            return f.Method;
        }

        private static MethodInfo GetMethodInfo<T1, T2, T3>(Func<T1, T2, T3> f, T1 unused1, T2 unused2)
        {
            return f.Method;
        }

        private static MethodInfo GetMethodInfo<T1, T2, T3, T4>(Func<T1, T2, T3, T4> f, T1 unused1, T2 unused2,
            T3 unused3)
        {
            return f.Method;
        }

        private static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> f, T1 unused1, T2 unused2,
            T3 unused3, T4 unused4)
        {
            return f.Method;
        }

        private static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7> f,
            T1 unused1, T2 unused2, T3 unused3, T4 unused4, T5 unused5, T6 unused6)
        {
            return f.Method;
        }
    }
}