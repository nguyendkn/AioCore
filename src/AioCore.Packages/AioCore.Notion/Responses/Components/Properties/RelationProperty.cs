using Newtonsoft.Json;

namespace AioCore.Notion.Responses.Components.Properties;

public class RelationProperty : Property
{
    public override PropertyType Type => PropertyType.Relation;

    [JsonProperty("relation")]
    public List<Relation> Relation { get; set; }
}