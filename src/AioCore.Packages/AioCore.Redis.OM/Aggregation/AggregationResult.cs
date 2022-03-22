namespace AioCore.Redis.OM.Aggregation
{
    public class AggregationResult<T> : IAggregationResult
    {
        private AggregationResult(RedisReply res)
        {
            Aggregations = new Dictionary<string, RedisReply>();
            var arr = res.ToArray();
            for (var i = 0; i < arr.Length; i += 2)
            {
                Aggregations.Add(arr[i], arr[i + 1]);
            }
        }

        public T? RecordShell { get; } = default;


        public IDictionary<string, RedisReply> Aggregations { get; }


        public RedisReply this[string key] => Aggregations[key];


        internal static IEnumerable<AggregationResult<T>> FromRedisResult(RedisReply res)
        {
            var arr = res.ToArray();
            for (var i = 1; i < arr.Length; i++)
            {
                yield return new AggregationResult<T>(arr[i]);
            }
        }
    }


    internal class AggregationResult : IAggregationResult
    {
        private AggregationResult(RedisReply res)
        {
            Aggregations = new Dictionary<string, RedisReply>();
            var arr = res.ToArray();
            for (var i = 0; i < arr.Length; i += 2)
            {
                Aggregations.Add(arr[i], arr[i + 1]);
            }
        }


        public Dictionary<string, RedisReply> Aggregations { get; }


        public RedisReply this[string? key] => Aggregations[key!];


        public static IEnumerable<AggregationResult> FromRedisResult(RedisReply res)
        {
            var arr = res.ToArray();
            for (var i = 1; i < arr.Length; i++)
            {
                yield return new AggregationResult(arr[i]);
            }
        }
    }
}