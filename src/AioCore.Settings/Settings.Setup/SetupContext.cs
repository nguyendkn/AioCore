using Package.Mongo;
using Shared.Types.ValueObjects;

namespace Settings.Setup;

public class SetupContext : MongoContext
{
    public SetupContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, "Setup.Settings")
    {
    }
}