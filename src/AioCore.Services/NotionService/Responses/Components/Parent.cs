using Newtonsoft.Json;

namespace AioCore.Services.NotionService.Responses.Components;

public class Parent
{
    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("database_id")] public string DatabaseId { get; set; } = default!;
}