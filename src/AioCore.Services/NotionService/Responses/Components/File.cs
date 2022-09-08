using Newtonsoft.Json;

namespace AioCore.Services.NotionService.Responses.Components;

public class File
{
    [JsonProperty("url")]
    public string Url { get; set; } = default!;

    [JsonProperty("expiry_time")]
    public DateTime ExpiryTime { get; set; }
}