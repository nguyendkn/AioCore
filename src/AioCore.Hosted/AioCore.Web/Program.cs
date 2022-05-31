using MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAioCore(builder.Configuration,
    (services, appSettings) =>
    {
        services.AddMediatR(
            typeof(Plugin.Cache.Assembly).Assembly,
            typeof(Plugin.Layout.Assembly).Assembly,
            typeof(Plugin.Seo.Assembly).Assembly,
            typeof(Plugin.Setup.Assembly).Assembly,
            typeof(Plugin.Slug.Assembly).Assembly
        );
    });
await builder.Build().UseAioCore().RunAsync();