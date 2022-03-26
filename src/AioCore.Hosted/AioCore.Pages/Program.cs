using AioCore.Fluid.Core;
using AioCore.Mongo.OM.MongoCore;
using AioCore.Pages;
using MediatR;
using Shared.Objects;
using Shared.Objects.MediatrRequests.Commands;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");

services.AddHttpClient();
services.AddMediatR(Assemblies.Load.ToArray());
services.AddFluidCore();
services.AddMongoContext<AioCoreContext>(connectionString, "aiocore");
var app = builder.Build();

app.MapGet("/", async (string? slug, IMediator mediator) =>
{
    var template = await mediator.Send(new LoadTemplateCommand(slug));
    return !string.IsNullOrEmpty(template.Error)
        ? Results.BadRequest(template.Error)
        : Results.Content(template.Rendered, "text/html");
});

app.MapGet("/{slug}", async (string? slug, IMediator mediator) =>
{
    var template = await mediator.Send(new LoadTemplateCommand(slug));
    return !string.IsNullOrEmpty(template.Error)
        ? Results.BadRequest(template.Error)
        : Results.Content(template.Rendered, "text/html");
});

app.Run();