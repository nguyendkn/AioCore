using Newtonsoft.Json;

namespace AioCore.Services.GraphQueries.FilterTypes;

public class EqualFilterType : GraphFilter
{
    public override GraphFilterType Type => GraphFilterType.Equal;

    [JsonProperty("value")]
    public object Value { get; set; } = default!;
}