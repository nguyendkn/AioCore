using Newtonsoft.Json;

namespace AioCore.Services.NotionService.Responses.Components;

public class Text
{
    [JsonProperty("content")] public string Content { get; set; } = default!;

    [JsonProperty("link")] public TextLink? Link { get; set; }
}