using Newtonsoft.Json;

namespace AioCore.Services.GraphQueries.FilterTypes;

public class NotEqualFilterType : GraphFilter
{
    public override GraphFilterType Type => GraphFilterType.NotEqual;

    [JsonProperty("value")] public object Value { get; set; } = default!;
}