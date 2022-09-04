using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class Relation
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("relation")]
    public List<string> Relations { get; set; }
}