using AioCore.Notion.Responses._Globals;
using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class Block
{
    [JsonProperty("object")] public string Object { get; set; } = "block";

    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("created_time")] public DateTime CreatedTime { get; set; }

    [JsonProperty("last_edited_time")] public DateTime LastEditedTime { get; set; }

    [JsonProperty("created_by")] public CreatedBy CreatedBy { get; set; }

    [JsonProperty("last_edited_by")] public LastEditedBy LastEditedBy { get; set; }

    [JsonProperty("has_children")] public bool HasChildren { get; set; }

    [JsonProperty("cover")] public object Cover { get; set; }

    [JsonProperty("icon")] public object Icon { get; set; }

    [JsonProperty("parent")] public Parent Parent { get; set; }

    [JsonProperty("archived")] public bool Archived { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("properties")] public Properties Properties { get; set; }

    [JsonProperty("url")] public string Url { get; set; }

    [JsonProperty("heading_2")] public Heading2 Heading2 { get; set; }

    [JsonProperty("image")] public Image Image { get; set; }

    [JsonProperty("paragraph")] public Paragraph Paragraph { get; set; }
}