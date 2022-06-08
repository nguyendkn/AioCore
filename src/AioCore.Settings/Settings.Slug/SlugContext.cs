using Package.Mongo;
using Shared.Types.ValueObjects;

namespace Settings.Slug;

public class SlugContext : MongoContext
{
    public SlugContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, "Slug.Settings")
    {
    }
}