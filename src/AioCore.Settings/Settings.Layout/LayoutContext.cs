using Package.Mongo;
using Shared.Types.ValueObjects;

namespace Settings.Layout;

public class LayoutContext : MongoContext
{
    public LayoutContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, "Layout.Settings")
    {
    }
}