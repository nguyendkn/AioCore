using AioCore.Notion.Responses.Components;
using Newtonsoft.Json;

namespace AioCore.Notion.Responses._Globals;

public class Database
{
    [JsonProperty("object")] public string Object { get; set; }

    [JsonProperty("results")] public List<Block> Results { get; set; }

    [JsonProperty("next_cursor")] public object NextCursor { get; set; }

    [JsonProperty("has_more")] public bool HasMore { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("page")] public object Page { get; set; }
}

public class Name
{
    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("title")] public List<Title> Title { get; set; }
}

public class Properties
{
}



