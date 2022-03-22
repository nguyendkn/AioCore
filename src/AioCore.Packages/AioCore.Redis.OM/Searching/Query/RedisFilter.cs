namespace AioCore.Redis.OM.Searching.Query
{
    public class RedisFilter : QueryOption
    {
        private readonly string _fieldName;


        private readonly int _min;


        private readonly int _max;


        public RedisFilter(string fieldName, int min = int.MinValue, int max = int.MaxValue)
        {
            _fieldName = fieldName;
            _min = min;
            _max = max;
        }


        internal override IEnumerable<string?> SerializeArgs
        {
            get
            {
                var ret = new List<string>
                {
                    "FILTER",
                    _fieldName,
                    _min.ToString(),
                    _max.ToString()
                };
                return ret.ToArray();
            }
        }
    }
}