using Newtonsoft.Json;

namespace AioCore.Services.GraphQueries.FilterTypes;

public class LessThanFilterType : GraphFilter
{
    public override GraphFilterType Type => GraphFilterType.LessThan;
    
    [JsonProperty("value")]
    public object Value { get; set; } = default!;
}