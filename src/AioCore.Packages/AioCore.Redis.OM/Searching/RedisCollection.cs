using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using AioCore.Redis.OM.Contracts;
using AioCore.Redis.OM.Modeling;

namespace AioCore.Redis.OM.Searching
{
    public class RedisCollection<T> : IRedisCollection<T>
        where T : notnull
    {
        private readonly IRedisConnection _connection;

        public RedisCollection(IRedisConnection connection, int chunkSize = 100)
        {
            var t = typeof(T);
            var rootAttribute = t.GetCustomAttribute<DocumentAttribute>();
            if (rootAttribute == null)
            {
                throw new ArgumentException(
                    "The root attribute of a Redis Collection must be decorated with a DocumentAttribute");
            }

            ChunkSize = chunkSize;
            _connection = connection;
            StateManager = new RedisCollectionStateManager(rootAttribute);
            Initialize(new RedisQueryProvider(connection, StateManager, rootAttribute, ChunkSize), null);
        }

        internal RedisCollection(RedisQueryProvider provider, Expression expression,
            RedisCollectionStateManager stateManager, int chunkSize = 100)
        {
            StateManager = stateManager;
            _connection = provider.Connection;
            ChunkSize = chunkSize;
            Initialize(provider, expression);
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; private set; } = default!;

        public IQueryProvider Provider { get; private set; } = default!;

        public RedisCollectionStateManager StateManager { get; }

        public int ChunkSize { get; }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            var provider = (RedisQueryProvider) Provider;
            var res = provider.ExecuteQuery<T>(expression);
            return res.Documents.Values.Any();
        }

        public async Task Update(T? item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (item == null) throw new ArgumentNullException(nameof(item));
            var key = item.GetKey();
            var diffConstructed = StateManager.TryDetectDifferencesSingle(key, item, out var diff);
            if (diffConstructed)
            {
                if (diff!.Any())
                {
                    var args = new List<string>();
                    var scriptName = diff!.First().Script;
                    foreach (var update in diff!)
                    {
                        args.AddRange(update.SerializeScriptArgs());
                    }

                    await _connection.CreateAndEvalAsync(scriptName, new[] {key}, args.ToArray());
                }
            }
            else
            {
                await _connection.UnlinkAndSet(key, item, StateManager.DocumentAttribute.StorageType);
            }

            StateManager.InsertIntoSnapshot(key, item);
            StateManager.InsertIntoData(key, item);
        }


        public async Task Delete(T item)
        {
            var key = item.GetKey();
            await _connection.UnlinkAsync(key);
            StateManager.Remove(key);
        }


        public IEnumerator<T> GetEnumerator()
        {
            return new RedisCollectionEnumerator<T>(Expression, _connection, ChunkSize, StateManager);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return Provider.Execute<IEnumerable>(Expression).GetEnumerator();
        }


        public void Save()
        {
            var diff = StateManager.DetectDifferences();
            foreach (var (key, value) in diff)
            {
                if (!value.Any()) continue;
                var args = new List<string>();
                var scriptName = value.First().Script;
                foreach (var update in value)
                {
                    args.AddRange(update.SerializeScriptArgs());
                }

                _connection.CreateAndEval(scriptName, new List<string> {key}, args.ToArray());
            }
        }


        public async ValueTask SaveAsync()
        {
            var diff = StateManager.DetectDifferences();
            var tasks = new List<Task<int?>>();

            foreach (var (key, value) in diff)
            {
                if (!value.Any()) continue;
                var args = new List<string>();
                var scriptName = value.First().Script;
                foreach (var update in value)
                {
                    args.AddRange(update.SerializeScriptArgs());
                }

                if (value.First() is HashDiff && args.Count <= 2)
                {
                    continue;
                }

                tasks.Add(_connection.CreateAndEvalAsync(scriptName, new[] {key}, args.ToArray()));
            }

            await Task.WhenAll(tasks);
        }


        public string Insert(T item)
        {
            return ((RedisQueryProvider) Provider).Connection.Set(item);
        }


        public async Task<string> InsertAsync(T item)
        {
            return await ((RedisQueryProvider) Provider).Connection.SetAsync(item);
        }


        public T FindById(string id) => _connection.Get<T>(id)!;


        public async Task<T> FindByIdAsync(string id) => await _connection.GetAsync<T>(id);


        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var provider = (RedisQueryProvider) Provider;
            return new RedisCollectionEnumerator<T>(Expression, provider.Connection, ChunkSize, StateManager);
        }

        private void Initialize(RedisQueryProvider provider, Expression? expression)
        {
            if (expression != null && !typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentException($"Not assignable from {expression.Type} expression");
            }

            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Expression = expression ?? Expression.Constant(this);
        }
    }
}