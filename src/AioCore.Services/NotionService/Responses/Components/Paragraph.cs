using Newtonsoft.Json;

namespace AioCore.Services.NotionService.Responses.Components;

public class Paragraph
{
    [JsonProperty("rich_text")] public List<RichText> RichText { get; set; } = default!;

    [JsonProperty("color")] public string Color { get; set; } = default!;
}