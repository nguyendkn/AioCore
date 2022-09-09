using Newtonsoft.Json;

namespace AioCore.Services.GraphQueries.FilterTypes;

public class InFilterType : GraphFilter
{
    public override GraphFilterType Type => GraphFilterType.In;
    
    [JsonProperty("args")]
    public List<object> Arguments { get; set; } = default!;
}