using AioCore.Web;
using MediatR;
using Package.Mongo;
using Settings.Cache;
using Settings.Layout;
using Settings.Seo;
using Settings.Setup;
using Settings.Slug;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAioCore(builder.Configuration,
    (services, appSettings) =>
    {
        var connectionString = appSettings.ConnectionStrings.DefaultConnection;
        services.AddMongoContext<CacheContext>(connectionString, CacheContext.DatabaseName);
        services.AddMongoContext<LayoutContext>(connectionString, LayoutContext.DatabaseName);
        services.AddMongoContext<SeoContext>(connectionString, SeoContext.DatabaseName);
        services.AddMongoContext<SetupContext>(connectionString, SetupContext.DatabaseName);
        services.AddMongoContext<SlugContext>(connectionString, SlugContext.DatabaseName);
        services.AddMediatR(Assemblies.All);
    });
await builder.Build().UseAioCore().RunAsync();