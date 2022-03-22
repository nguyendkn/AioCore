namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public class GroupBy : IAggregationPredicate
    {
        public GroupBy(string[] properties)
        {
            Properties = properties;
        }


        public string[] Properties { get; set; }


        public IEnumerable<string> Serialize()
        {
            var ret = new List<string>
            {
                "GROUPBY",
                Properties.Length.ToString(),
            };
            ret.AddRange(Properties.Select(property => $"@{property}"));
            return ret.ToArray();
        }
    }
}