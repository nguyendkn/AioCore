namespace AioCore.Redis.OM.Searching.Query
{
    public class SearchLimit : QueryOption
    {
        public int Offset { get; set; }

        public int Number { get; set; } = 10;

        internal override IEnumerable<string?> SerializeArgs
        {
            get
            {
                return new[]
                {
                    "LIMIT",
                    Offset.ToString(),
                    Number.ToString(),
                };
            }
        }
    }
}