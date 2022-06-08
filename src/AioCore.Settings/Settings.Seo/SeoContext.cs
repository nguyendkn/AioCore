using MongoDB.Driver;
using Package.Mongo;
using Shared.Types.ValueObjects;

namespace Settings.Seo;

public class SeoContext : MongoContext
{
    public SeoContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, "Seo.Settings")
    {
    }
}