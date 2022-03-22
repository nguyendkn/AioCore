using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using AioCore.Redis.OM.Contracts;
using AioCore.Redis.OM.Modeling;
using AioCore.Redis.OM.Searching;

namespace AioCore.Redis.OM.Aggregation
{
    public class RedisAggregationSet<T> : IQueryable<AggregationResult<T>>, IAsyncEnumerable<AggregationResult<T>>
    {
        private readonly int _chunkSize;
        private bool _useCursor;


        public RedisAggregationSet(IRedisConnection connection, bool useCursor = false, int chunkSize = 1000)
        {
            var t = typeof(T);
            var rootAttribute = t.GetCustomAttribute<DocumentAttribute>();
            if (rootAttribute == null)
            {
                throw new ArgumentException(
                    "The root attribute of an AggregationSet must be decorated with a DocumentAttribute");
            }

            _chunkSize = chunkSize;
            Initialize(new RedisQueryProvider(connection, rootAttribute, _chunkSize), null, useCursor);
        }


        internal RedisAggregationSet(RedisQueryProvider provider, bool useCursor = false, int chunkSize = 1000)
        {
            _chunkSize = chunkSize;
            Initialize(provider, null, useCursor);
        }


        internal RedisAggregationSet(RedisAggregationSet<T> source, Expression exp)
        {
            _chunkSize = source._chunkSize;
            Initialize((RedisQueryProvider) source.Provider, exp, source._useCursor);
        }


        public Type ElementType
        {
            get => typeof(AggregationResult<T>);
        }


        public Expression Expression { get; private set; } = default!;


        public IQueryProvider Provider { get; private set; } = default!;


        public IEnumerator<AggregationResult<T>> GetEnumerator()
        {
            var provider = (RedisQueryProvider) Provider;
            return new AggregationEnumerator<T>(Expression, provider.Connection, useCursor: _useCursor,
                chunkSize: _chunkSize);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            var provider = (RedisQueryProvider) Provider;
            return new AggregationEnumerator<T>(Expression, provider.Connection, useCursor: _useCursor,
                chunkSize: _chunkSize);
        }


        public IAsyncEnumerator<AggregationResult<T>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var provider = (RedisQueryProvider) Provider;
            return new AggregationEnumerator<T>(Expression, provider.Connection, useCursor: _useCursor,
                chunkSize: _chunkSize);
        }


        public async ValueTask<List<AggregationResult<T>>> ToListAsync()
        {
            var retList = new List<AggregationResult<T>>();
            await foreach (var item in this)
            {
                retList.Add(item);
            }

            return retList;
        }


        public async ValueTask<AggregationResult<T>[]> ToArrayAsync()
            => (await ToListAsync()).ToArray();

        private void Initialize(RedisQueryProvider provider, Expression? expression, bool useCursor)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Expression = expression ?? Expression.Constant(this);
            _useCursor = useCursor;
        }
    }
}