using Package.Mongo;
using Shared.Types.ValueObjects;

namespace Settings.Cache;

public class CacheContext : MongoContext
{
    public CacheContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, "Cache.Settings")
    {
    }
}