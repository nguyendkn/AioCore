using System.Runtime.Serialization;
using AioCore.Services.NotionService.Responses.Components.Properties;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AioCore.Services.NotionService.Responses.Components;

[JsonConverter(typeof(JsonSubtypes), "type")]
[JsonSubtypes.KnownSubType(typeof(RelationProperty), PropertyType.Relation)]
[JsonSubtypes.KnownSubType(typeof(RichTextProperty), PropertyType.RichText)]
[JsonSubtypes.KnownSubType(typeof(TitleProperty), PropertyType.Title)]
public class Property
{
    [JsonProperty("id")] public string Id { get; set; } = default!;

    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public virtual PropertyType Type { get; set; }

    [JsonProperty("name")] public string Name { get; set; } = default!;
}

public enum PropertyType
{
    [EnumMember(Value = null)] Unknown,

    [EnumMember(Value = "title")] Title,

    [EnumMember(Value = "rich_text")] RichText,

    [EnumMember(Value = "number")] Number,

    [EnumMember(Value = "select")] Select,

    [EnumMember(Value = "multi_select")] MultiSelect,

    [EnumMember(Value = "date")] Date,

    [EnumMember(Value = "people")] People,

    [EnumMember(Value = "files")] Files,

    [EnumMember(Value = "checkbox")] Checkbox,

    [EnumMember(Value = "url")] Url,

    [EnumMember(Value = "email")] Email,

    [EnumMember(Value = "phone_number")] PhoneNumber,

    [EnumMember(Value = "formula")] Formula,

    [EnumMember(Value = "relation")] Relation,

    [EnumMember(Value = "rollup")] Rollup,

    [EnumMember(Value = "created_time")] CreatedTime,

    [EnumMember(Value = "created_by")] CreatedBy,

    [EnumMember(Value = "last_edited_by")] LastEditedBy,

    [EnumMember(Value = "last_edited_time")]
    LastEditedTime,

    [EnumMember(Value = "status")] Status,
}