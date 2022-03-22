using System.Collections;
using System.Linq.Expressions;
using AioCore.Redis.OM.Common;
using AioCore.Redis.OM.Contracts;

namespace AioCore.Redis.OM.Aggregation
{
    internal class AggregationEnumerator<T> : IEnumerator<AggregationResult<T>>, IAsyncEnumerator<AggregationResult<T>>
    {
        private readonly IRedisConnection _connection;
        private readonly RedisAggregation _aggregation;
        private readonly bool _useCursor;
        private readonly int _chunkSize;
        private AggregationResult<T>[] _chunk;
        private int _index;
        private int _cursor = -1;
        private bool _queried;


        internal AggregationEnumerator(Expression exp, IRedisConnection connection, int chunkSize = 1000,
            bool useCursor = false)
        {
            _chunk = Array.Empty<AggregationResult<T>>();
            _index = 0;
            _chunkSize = chunkSize;
            _aggregation = ExpressionTranslator.BuildAggregationFromExpression(exp, typeof(T));
            _connection = connection;
            _useCursor = useCursor;
        }


        public AggregationResult<T> Current => _chunk[_index];


        object IEnumerator.Current => _chunk[_index];

        private string[] NextChunkArgs =>
            new[]
            {
                "READ",
                _aggregation.IndexName,
                _cursor.ToString(),
                "COUNT",
                _chunkSize.ToString(),
            }!;

        private string[] SerializedArgs
        {
            get
            {
                var serializedArgs = _aggregation.Serialize().ToList();
                if (!_useCursor) return serializedArgs.ToArray();
                serializedArgs.Add("WITHCURSOR");
                serializedArgs.Add("COUNT");
                serializedArgs.Add(_chunkSize.ToString());

                return serializedArgs.ToArray();
            }
        }


        public void Dispose()
        {
            if (_cursor == 0)
            {
                return;
            }

            try
            {
                var args = new[] {"DEL", _aggregation.IndexName, _cursor.ToString()};
                _connection.Execute("FT.CURSOR", args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(
                    "Unable to delete cursor, most likely because it had already been exhausted");
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }


        public async ValueTask DisposeAsync()
        {
            if (_cursor != 0)
            {
                try
                {
                    var args = new[] {"DEL", _aggregation.IndexName, _cursor.ToString()};
                    await _connection.ExecuteAsync("FT.CURSOR", args);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex);
                }
            }
        }


        public bool MoveNext()
        {
            if (_index + 1 < _chunk.Length)
            {
                _index++;
                return true;
            }

            if (_useCursor)
            {
                return _cursor switch
                {
                    -1 => StartEnumeration(),
                    0 => false,
                    _ => ReadNextChunk()
                };
            }

            return !_queried && StartEnumeration();
        }


        public async ValueTask<bool> MoveNextAsync()
        {
            if (_index + 1 < _chunk.Length)
            {
                _index++;
                return true;
            }

            if (_useCursor)
            {
                return _cursor switch
                {
                    -1 => await StartEnumerationAsync(),
                    0 => false,
                    _ => await ReadNextChunkAsync()
                };
            }

            if (!_queried)
            {
                return await StartEnumerationAsync();
            }

            return false;
        }


        public void Reset()
        {
            _cursor = -1;
            _index = 0;
            _chunk = Array.Empty<AggregationResult<T>>();
        }

        private void ParseResult(RedisReply res)
        {
            if (_useCursor)
            {
                var arr = res.ToArray();
                _cursor = arr[1];
                _chunk = AggregationResult<T>.FromRedisResult(arr[0]).ToArray();
            }
            else
            {
                _chunk = AggregationResult<T>.FromRedisResult(res).ToArray();
            }

            _index = 0;
            _queried = true;
        }

        private async ValueTask<bool> ReadNextChunkAsync()
        {
            var res = await _connection.ExecuteAsync("FT.CURSOR", NextChunkArgs);
            ParseResult(res);
            return _index < _chunk.Length;
        }

        private async ValueTask<bool> StartEnumerationAsync()
        {
            var args = SerializedArgs;
            var res = await _connection.ExecuteAsync("FT.AGGREGATE", args);
            ParseResult(res);
            return _index < _chunk.Length;
        }

        private bool ReadNextChunk()
        {
            var args = NextChunkArgs;
            var res = _connection.Execute("FT.CURSOR", args);
            ParseResult(res);
            return _index < _chunk.Length;
        }

        private bool StartEnumeration()
        {
            var args = SerializedArgs;
            var res = _connection.Execute("FT.AGGREGATE", args);
            ParseResult(res);
            return _index < _chunk.Length;
        }
    }
}