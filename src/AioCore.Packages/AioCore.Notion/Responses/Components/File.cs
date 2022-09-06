using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class File
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("expiry_time")]
    public DateTime ExpiryTime { get; set; }
}