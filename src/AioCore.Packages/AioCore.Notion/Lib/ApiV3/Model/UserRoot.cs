namespace AioCore.Notion.Lib.ApiV3.Model
{
    public class UserRoot : BaseModel
    {
        public List<Guid> SpaceViews => Value["space_views"].ToObject<List<Guid>>();
    }
}
