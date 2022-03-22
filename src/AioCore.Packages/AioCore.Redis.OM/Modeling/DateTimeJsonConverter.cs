using System.Text.Json;
using System.Text.Json.Serialization;

namespace AioCore.Redis.OM.Modeling
{
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var val = reader.GetString();
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddMilliseconds(long.Parse(val!));
        }


        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(new DateTimeOffset(value).ToUnixTimeMilliseconds().ToString());
        }
    }
}