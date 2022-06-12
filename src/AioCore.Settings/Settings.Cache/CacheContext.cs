using Package.Mongo;
using Shared.Types.ValueObjects;

namespace Settings.Cache;

public class CacheContext : MongoContext
{
    public const string DatabaseName = "Cache.Settings";

    public CacheContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, DatabaseName)
    {
    }
}