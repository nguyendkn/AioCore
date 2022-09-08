using Newtonsoft.Json;

namespace AioCore.Services.NotionService.Responses.Components;

public class Href
{
    [JsonProperty("id")] public string Id { get; set; } = default!;

    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("rich_text")] public List<RichText> RichText { get; set; } = default!;
}