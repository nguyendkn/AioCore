using Newtonsoft.Json;

namespace AioCore.Services.GraphQueries.FilterTypes;

public class NotInFilterType : GraphFilter
{
    public override GraphFilterType Type => GraphFilterType.NotIn;
    
    [JsonProperty("args")]
    public List<object> Arguments { get; set; } = default!;
}