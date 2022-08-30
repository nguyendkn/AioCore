using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class Annotations
{
    [JsonProperty("bold")] public bool Bold { get; set; }

    [JsonProperty("italic")] public bool Italic { get; set; }

    [JsonProperty("strikethrough")] public bool Strikethrough { get; set; }

    [JsonProperty("underline")] public bool Underline { get; set; }

    [JsonProperty("code")] public bool Code { get; set; }

    [JsonProperty("color")] public string Color { get; set; }

    [JsonIgnore]
    public bool HasAnnotation =>
        Bold || Italic || Strikethrough || Underline || Code || (Color != null && Color != "default");
}