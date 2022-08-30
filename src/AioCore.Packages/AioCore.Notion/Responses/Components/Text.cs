using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class Text
{
    [JsonProperty("content")] public string Content { get; set; }

    [JsonProperty("link")] public TextLink Link { get; set; }
}