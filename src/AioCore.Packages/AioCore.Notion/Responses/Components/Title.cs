using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class Title
{
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("text")] public Text Text { get; set; }

    [JsonProperty("annotations")] public Annotations Annotations { get; set; }

    [JsonProperty("plain_text")] public string PlainText { get; set; }

    [JsonProperty("href")] public object Href { get; set; }
}