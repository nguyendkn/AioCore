using Newtonsoft.Json;

namespace AioCore.Services.NotionService.Responses.Components.Properties;

public class TitleProperty : Property
{
    public override PropertyType Type => PropertyType.Title;

    [JsonProperty("title")]
    public List<Title> Title { get; set; } = default!;
}