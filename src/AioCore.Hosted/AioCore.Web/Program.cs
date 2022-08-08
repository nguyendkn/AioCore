using AioCore.Shared.Extensions;
using AioCore.Shared.ValueObjects;
using AioCore.Web.Helpers;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddAntDesign();
services.AddAioContext(appSettings);
services.AddMapper<MappingProfile>();
services.AddMediatR(typeof(AioCore.Read.Assembly));
services.AddMediatR(typeof(AioCore.Write.Assembly));
var app = builder.Build();
app.UseAioCore();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();