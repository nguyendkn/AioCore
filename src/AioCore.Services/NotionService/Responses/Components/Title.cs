using Newtonsoft.Json;

namespace AioCore.Services.NotionService.Responses.Components;

public class Title
{
    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("text")] public Text Text { get; set; } = default!;

    [JsonProperty("annotations")] public Annotations Annotations { get; set; } = default!;

    [JsonProperty("plain_text")] public string PlainText { get; set; } = default!;

    [JsonProperty("href")] public object Href { get; set; } = default!;
}