using Newtonsoft.Json;

namespace AioCore.Notion.Lib.ApiV3.Model
{
    public class Cursor
    {
        [JsonProperty("stack")]
        public List<List<CursorStack>> Stack { get; set; }
    }
}
