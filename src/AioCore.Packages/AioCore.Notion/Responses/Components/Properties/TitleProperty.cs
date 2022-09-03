using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components.Properties;

public class TitleProperty : Property
{
    public override PropertyType Type => PropertyType.Title;

    [JsonProperty("title")]
    public List<Title> Title { get; set; }
}