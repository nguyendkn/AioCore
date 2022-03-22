namespace AioCore.Redis.OM.Searching.Query
{
    public enum QueryFlags
    {
        Nocontent = 1,


        Verbatim = 2,


        NoStopWords = 4,


        WithScores = 8,


        WithPayloads = 16,


        WithSortKeys = 32,
    }
}