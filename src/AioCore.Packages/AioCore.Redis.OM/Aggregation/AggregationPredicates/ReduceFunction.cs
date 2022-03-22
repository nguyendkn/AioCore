namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public enum ReduceFunction
    {
        COUNT = 0,


        COUNT_DISTINCT = 1,


        COUNT_DISTINCTISH = 2,


        SUM = 3,


        MIN = 4,


        MAX = 5,


        AVG = 6,


        STDDEV = 7,


        QUANTILE = 8,


        TOLIST = 9,


        FIRST_VALUE = 10,


        RANDOM_SAMPLE = 11,
    }
}