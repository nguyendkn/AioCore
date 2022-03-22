using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using AioCore.Redis.OM.Common;
using AioCore.Redis.OM.Contracts;
using AioCore.Redis.OM.Modeling;
using AioCore.Redis.OM.Searching.Query;

namespace AioCore.Redis.OM.Searching
{
    internal class RedisCollectionEnumerator<T> : IEnumerator<T>, IAsyncEnumerator<T>
        where T : notnull
    {
        private readonly RedisQuery _query;
        private readonly Type? _primitiveType;
        private readonly bool _limited;
        private readonly IRedisConnection _connection;
        private readonly RedisCollectionStateManager _stateManager;
        private SearchResponse<T> _records = new(new RedisReply(new RedisReply[] {0}));
        private bool _started;
        private int _index = -1;

        public RedisCollectionEnumerator(Expression exp, IRedisConnection connection, int chunkSize,
            RedisCollectionStateManager stateManager)
        {
            Type rootType;
            var t = typeof(T);
            var documentDefinition = t.GetCustomAttribute<DocumentAttribute>();
            if (documentDefinition == null)
            {
                _primitiveType = t;
                rootType = GetRootType((MethodCallExpression) exp);
            }
            else
            {
                rootType = t;
            }

            _query = ExpressionTranslator.BuildQueryFromExpression(exp, rootType);
            if (_query.Limit != null)
            {
                _limited = true;
            }
            else
            {
                _query.Limit = new SearchLimit {Offset = 0, Number = chunkSize};
            }

            _connection = connection;
            _stateManager = stateManager;
        }

        public T Current => _records[_index];

        object? IEnumerator.Current => _records[_index];

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }


        public bool MoveNext()
        {
            if (_index + 1 < _records.Documents.Count)
            {
                _index += 1;
                return true;
            }

            switch (_started)
            {
                case true when _limited:
                case true when _records.Documents.Count < _query!.Limit!.Number:
                    return false;
                default:
                    return GetNextChunk();
            }
        }


        public async ValueTask<bool> MoveNextAsync()
        {
            if (_index + 1 < _records.Documents.Count)
            {
                _index += 1;
                return true;
            }

            switch (_started)
            {
                case true when _limited:
                case true when _records.Documents.Count < _query!.Limit!.Number:
                    return false;
                default:
                    return await GetNextChunkAsync();
            }
        }


        public void Reset()
        {
            _started = false;
            _index = -1;
            _records = new SearchResponse<T>(new RedisReply(new RedisReply[] {0}));
            if (_query.Limit != null)
            {
                _query.Limit.Offset = 0;
            }
        }

        private static Type GetRootType(MethodCallExpression expression)
        {
            while (expression.Arguments[0] is MethodCallExpression innerExpression)
            {
                expression = innerExpression;
            }

            return expression.Arguments[0].Type.GenericTypeArguments[0];
        }

        private bool GetNextChunk()
        {
            if (_started)
            {
                _query!.Limit!.Offset = _query.Limit.Offset + _query.Limit.Number;
            }

            var res = _connection.SearchRawResult(_query);
            _records = new SearchResponse<T>(res);
            _index = 0;
            _started = true;
            ConcatenateRecords();
            return _index < _records.Documents.Count;
        }

        private async ValueTask<bool> GetNextChunkAsync()
        {
            if (_started)
            {
                _query!.Limit!.Offset = _query.Limit.Offset + _query.Limit.Number;
            }

            _records = await _connection.SearchAsync<T>(_query);
            _index = 0;
            _started = true;
            ConcatenateRecords();
            return _index < _records.Documents.Count;
        }

        private void ConcatenateRecords()
        {
            foreach (var (key, value) in _records.Documents)
            {
                if (_stateManager.Data.ContainsKey(key) || _primitiveType != null)
                {
                    continue;
                }

                _stateManager.InsertIntoData(key, value);
                _stateManager.InsertIntoSnapshot(key, value);
            }
        }
    }
}