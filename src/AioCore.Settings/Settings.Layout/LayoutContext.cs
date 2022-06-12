using Package.Mongo;
using Shared.Types.ValueObjects;

namespace Settings.Layout;

public class LayoutContext : MongoContext
{
    public const string DatabaseName = "Layout.Settings";

    public LayoutContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, DatabaseName)
    {
    }
}