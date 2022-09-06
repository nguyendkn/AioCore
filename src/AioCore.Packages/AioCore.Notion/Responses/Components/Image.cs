using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components;

public class Image
{
    public const string ExternalType = "external";
    public const string FileType = "file";
    
    [JsonProperty("caption")] public List<string> Caption { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("external")] public External External { get; set; }
    
    [JsonProperty("file")] public File File { get; set; }
}