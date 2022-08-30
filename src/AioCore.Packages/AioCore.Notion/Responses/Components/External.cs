using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class External
{
    [JsonProperty("url")] public string Url { get; set; }
}