namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    public static class ApplyFunctions
    {
        public static bool Exists(object field)
        {
            return true;
        }


        public static string FormatTimestamp(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToString("%FT%TZ");
        }


        public static string FormatTimestamp(long timestamp, string format)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToString(format);
        }


        public static long ParseTime(string timestamp)
        {
            return new DateTimeOffset(DateTime.Parse(timestamp)).ToUnixTimeSeconds();
        }


        public static long ParseTime(string timestamp, string format)
        {
            return ParseTime(timestamp);
        }


        public static long Day(long timestamp)
        {
            var time = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return new DateTimeOffset(new DateTime(time.Year, time.Month, time.Day, 0, 0, 0)).ToUnixTimeSeconds();
        }


        public static long Hour(long timestamp)
        {
            var time = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return new DateTimeOffset(new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0))
                .ToUnixTimeSeconds();
        }


        public static long Minute(long timestamp)
        {
            var time = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return new DateTimeOffset(new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0))
                .ToUnixTimeSeconds();
        }


        public static long Month(long timestamp)
        {
            var time = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return new DateTimeOffset(new DateTime(time.Year, time.Month, 0, 0, 0, 0)).ToUnixTimeSeconds();
        }


        public static long DayOfWeek(long timestamp)
        {
            return (long) DateTimeOffset.FromUnixTimeSeconds(timestamp).DayOfWeek;
        }


        public static long DayOfMonth(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).Day;
        }


        public static long DayOfYear(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DayOfYear;
        }


        public static long Year(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).Year;
        }


        public static long MonthOfYear(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).Month;
        }
    }
}