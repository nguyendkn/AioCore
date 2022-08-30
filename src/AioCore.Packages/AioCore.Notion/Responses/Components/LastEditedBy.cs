using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class LastEditedBy
{
    [JsonProperty("object")] public string Object { get; set; } = default!;

    [JsonProperty("id")] public string Id { get; set; } = default!;
}