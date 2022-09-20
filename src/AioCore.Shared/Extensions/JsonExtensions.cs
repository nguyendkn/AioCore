using Newtonsoft.Json;

namespace AioCore.Shared.Extensions;

public static class JsonExtensions
{
    public static string Serialize<T>(this T input)
    {
        return JsonConvert.SerializeObject(input);
    }

    public static T Deserialize<T>(this string? input)
    {
        return !string.IsNullOrEmpty(input) ? JsonConvert.DeserializeObject<T>(input)! : default!;
    }
}