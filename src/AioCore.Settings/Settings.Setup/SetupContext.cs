using Package.Mongo;
using Settings.Setup.Domain.ConfigurationAggregate;
using Shared.Types.ValueObjects;

namespace Settings.Setup;

public class SetupContext : MongoContext
{
    public const string DatabaseName = "Setup.Settings";

    public SetupContext(AppSettings appSettings) :
        base(appSettings.ConnectionStrings.DefaultConnection, DatabaseName)
    {
    }

    public MongoSet<Configuration> Configurations { get; set; } = default!;
}