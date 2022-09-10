using AioCore.Services.GraphQueries.FilterTypes;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AioCore.Services.GraphQueries;

[JsonConverter(typeof(JsonSubtypes), "type")]
[JsonSubtypes.KnownSubType(typeof(EqualFilterType), GraphFilterType.Equal)]
[JsonSubtypes.KnownSubType(typeof(GatherThanFilterType), GraphFilterType.GatherThan)]
[JsonSubtypes.KnownSubType(typeof(GreaterThanOrEqualFilterType), GraphFilterType.GatherThanOrEqual)]
[JsonSubtypes.KnownSubType(typeof(InFilterType), GraphFilterType.In)]
[JsonSubtypes.KnownSubType(typeof(LessThanFilterType), GraphFilterType.LessThan)]
[JsonSubtypes.KnownSubType(typeof(LessThanFilterType), GraphFilterType.LessThanOrEqual)]
[JsonSubtypes.KnownSubType(typeof(NotEqualFilterType), GraphFilterType.NotEqual)]
[JsonSubtypes.KnownSubType(typeof(NotInFilterType), GraphFilterType.NotIn)]
public class GraphFilter
{
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public virtual GraphFilterType Type { get; set; }
}