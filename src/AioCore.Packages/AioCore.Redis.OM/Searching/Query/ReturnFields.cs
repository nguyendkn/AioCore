namespace AioCore.Redis.OM.Searching.Query
{
    public class ReturnFields : QueryOption
    {
        private readonly IEnumerable<string> _fields;


        public ReturnFields(IEnumerable<string> fields)
        {
            _fields = fields;
        }


        internal override IEnumerable<string?> SerializeArgs
        {
            get
            {
                var ret = new List<string> {"RETURN", _fields.Count().ToString()};
                ret.AddRange(_fields.Select(field => $"{field}"));
                return ret.ToArray();
            }
        }
    }
}