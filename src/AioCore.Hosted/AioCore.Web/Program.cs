using AioCore.Shared.Extensions;
using AioCore.Shared.Hangfire;
using AioCore.Shared.ValueObjects;
using AioCore.Web.Helpers;
using AioCore.Web.MiddleWares;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

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
services.AddBackgroundServicesAioCore(appSettings);
services.AddMediatR(typeof(AioCore.Read.Assembly));
services.AddMediatR(typeof(AioCore.Write.Assembly));
services.AddMediatR(typeof(AioCore.Request.Assembly));
var app = builder.Build();
app.UseAioCoreDatabase(appSettings);
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAioController();
app.UseStaticRender();
app.UseJobs(environment);
app.UseHangfire();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

