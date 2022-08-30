using Newtonsoft.Json.Linq;

namespace AioCore.Notion.Lib.ApiV3.Results
{
    public class GetUserTasksResult
    {
        public List<JObject> TaskIds { get; set; }
    }
}