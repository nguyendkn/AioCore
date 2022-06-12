using MongoDB.Driver;
using Package.Mongo;
using Shared.Types.ValueObjects;

namespace Settings.Seo;

public class SeoContext : MongoContext
{
    public const string DatabaseName = "Seo.Settings";

    public SeoContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, DatabaseName)
    {
    }
}