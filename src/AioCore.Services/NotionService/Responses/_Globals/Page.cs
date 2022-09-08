using AioCore.Services.NotionService.Responses.Components;
using Newtonsoft.Json;

namespace AioCore.Services.NotionService.Responses._Globals;

public class Page
{
    [JsonProperty("object")] public string Object { get; set; } = default!;

    [JsonProperty("results")] public List<Block?> Results { get; set; } = default!;

    [JsonProperty("next_cursor")] public object NextCursor { get; set; } = default!;

    [JsonProperty("has_more")] public bool HasMore { get; set; }

    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("block")] public object Block { get; set; } = default!;
}