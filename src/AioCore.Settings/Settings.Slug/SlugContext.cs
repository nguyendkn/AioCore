using Package.Mongo;
using Shared.Types.ValueObjects;

namespace Settings.Slug;

public class SlugContext : MongoContext
{
    public const string DatabaseName = "Slug.Settings";

    public SlugContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, DatabaseName)
    {
    }
}