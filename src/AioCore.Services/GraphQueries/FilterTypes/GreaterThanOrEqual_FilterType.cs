using Newtonsoft.Json;

namespace AioCore.Services.GraphQueries.FilterTypes;

public class GreaterThanOrEqualFilterType : GraphFilter
{
    public override GraphFilterType Type => GraphFilterType.GatherThan;
    
    [JsonProperty("value")]
    public object Value { get; set; } = default!;
}