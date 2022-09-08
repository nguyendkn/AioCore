using Newtonsoft.Json;

namespace AioCore.Services.NotionService.Responses.Components;

public class External
{
    [JsonProperty("url")] public string Url { get; set; } = default!;
}