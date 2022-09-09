using System.Runtime.Serialization;

namespace AioCore.Services.GraphQueries;

public enum GraphRequestType
{
    [EnumMember(Value = null)] Unknown,

    [EnumMember(Value = "get")] Get,

    [EnumMember(Value = "filter")] Filter,

    [EnumMember(Value = "post")] Post,

    [EnumMember(Value = "put")] Put,

    [EnumMember(Value = "delete")] Delete,
}