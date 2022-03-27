using AioCore.Fluid.Core;
using AioCore.Mongo.OM.MongoCore;
using AioCore.Web;
using MediatR;
using Shared.Objects;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddMediatR(Assemblies.Load.ToArray());
services.AddFluidCore();
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddMongoContext<AioCoreContext>(connectionString, "aiocore");
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();