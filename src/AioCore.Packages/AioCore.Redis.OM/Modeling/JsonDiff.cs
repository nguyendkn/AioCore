using Newtonsoft.Json.Linq;

namespace AioCore.Redis.OM.Modeling
{
    internal class JsonDiff : IObjectDiff
    {
        private readonly string _operation;
        private readonly string _path;
        private readonly JToken _value;

        internal JsonDiff(string operation, string path, JToken value)
        {
            _operation = operation;
            _path = path;
            _value = value;
        }

        public string Script => nameof(Scripts.JsonDiffResolution);


        public IEnumerable<string> SerializeScriptArgs()
        {
            return _value.Type == JTokenType.String
                ? new[] {_operation, _path, $"\"{_value}\""}
                : new[] {_operation, _path, _value.ToString()};
        }
    }
}