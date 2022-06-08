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
        services.AddMongoContext<CacheContext>();
        services.AddMongoContext<LayoutContext>();
        services.AddMongoContext<SeoContext>();
        services.AddMongoContext<SetupContext>();
        services.AddMongoContext<SlugContext>();
        services.AddMediatR(Assemblies.All);
    });
await builder.Build().UseAioCore().RunAsync();