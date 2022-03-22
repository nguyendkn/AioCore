namespace AioCore.Redis.OM.Modeling
{
    internal class HashDiff : IObjectDiff
    {
        private readonly IEnumerable<KeyValuePair<string, string>> _setFieldValuePairs;
        private readonly IEnumerable<string> _delValues;

        public HashDiff(IEnumerable<KeyValuePair<string, string>> setSetFieldValuePairs, IEnumerable<string> delValues)
        {
            _setFieldValuePairs = setSetFieldValuePairs;
            _delValues = delValues;
        }

        public string Script => nameof(Scripts.HashDiffResolution);

        public IEnumerable<string> SerializeScriptArgs()
        {
            var ret = new List<string> {_setFieldValuePairs.Count().ToString()};
            foreach (var (key, value) in _setFieldValuePairs)
            {
                ret.Add(key);
                ret.Add(value);
            }

            ret.AddRange(_delValues);
            return ret.ToArray();
        }
    }
}