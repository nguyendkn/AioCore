using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class RichText
{
    public const string TypeText = "text";
    public const string TypeMention = "mention";
    public const string TypeLink = "link";
    public const string TypeEquation = "equation";
    
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("text")] public Text Text { get; set; }

    [JsonProperty("annotations")] public Annotations Annotations { get; set; }

    [JsonProperty("plain_text")] public string PlainText { get; set; }

    [JsonProperty("href")] public string Href { get; set; }

    [JsonIgnore] public bool HasAttribute => Annotations?.HasAnnotation == true || !string.IsNullOrWhiteSpace(Href);

    [JsonIgnore] public bool HasStyle => !string.IsNullOrWhiteSpace(Annotations?.Color);
}