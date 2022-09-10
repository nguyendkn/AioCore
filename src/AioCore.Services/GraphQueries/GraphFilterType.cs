using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AioCore.Services.GraphQueries;

[JsonConverter(typeof(StringEnumConverter))]
public enum GraphFilterType
{
    [EnumMember(Value = null)] Unknown,

    [EnumMember(Value = "eq")] Equal,

    [EnumMember(Value = "gt")] GatherThan,
    
    [EnumMember(Value = "gte")] GatherThanOrEqual,

    [EnumMember(Value = "in")] In,

    [EnumMember(Value = "lt")] LessThan,
    
    [EnumMember(Value = "lte")] LessThanOrEqual,
    
    [EnumMember(Value = "ne")] NotEqual,

    [EnumMember(Value = "nin")] NotIn,
}