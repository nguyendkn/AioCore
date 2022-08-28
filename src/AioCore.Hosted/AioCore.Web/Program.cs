using AioCore.Shared.Extensions;
using AioCore.Shared.ValueObjects;
using AioCore.Web.Helpers;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

services.AddSingleton(appSettings);
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddAioController();
services.AddAntDesign();
services.AddAioContext(appSettings);
services.AddMapper<MappingProfile>();
services.AddSingletonAioCore();
services.AddScopedAioCore();
services.AddMediatR(typeof(AioCore.Read.Assembly));
services.AddMediatR(typeof(AioCore.Write.Assembly));
var app = builder.Build();
app.UseAioCoreDatabase();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAioController();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();