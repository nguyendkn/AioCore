using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AioCore.Services.GraphQueries;

public class GraphRequest
{
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public virtual GraphRequestType Type { get; set; }

    [JsonProperty("collection")]
    public string Collection { get; set; } = default!;
    
    [JsonProperty("data")]

    public Dictionary<string, object>? Data { get; set; }

    [JsonProperty("filters")]
    public Dictionary<string, GraphFilter>? Filters { get; set; }
}