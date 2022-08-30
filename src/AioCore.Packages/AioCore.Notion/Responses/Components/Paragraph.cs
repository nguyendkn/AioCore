using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class Paragraph
{
    [JsonProperty("rich_text")] public List<RichText> RichText { get; set; }

    [JsonProperty("color")] public string Color { get; set; }
}