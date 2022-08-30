using AioCore.Notion.Lib.ApiV3.Model;
using Newtonsoft.Json;

namespace AioCore.Notion.Lib.ApiV3.Requests
{
    public class LoadPageChunkRequest
    {
        [JsonProperty("chunkNumber")]
        public int ChunkNumber { get; set; }
        [JsonProperty("cursor")]
        public Cursor Cursor { get; set; }
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("pageId")]
        public Guid PageId { get; set; }
        [JsonProperty("verticalColumns")]
        public bool VerticalColumns { get; set; }
    }
}
