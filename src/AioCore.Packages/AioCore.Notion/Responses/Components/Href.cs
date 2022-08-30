using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class Href
{
    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("rich_text")] public List<RichText> RichText { get; set; }
}