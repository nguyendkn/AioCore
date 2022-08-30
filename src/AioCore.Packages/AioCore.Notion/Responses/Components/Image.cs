using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class Image
{
    [JsonProperty("caption")] public List<string> Caption { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("external")] public External External { get; set; }
}