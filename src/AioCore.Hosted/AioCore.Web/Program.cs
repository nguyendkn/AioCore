using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var environment = builder.Environment;

if (environment.IsDevelopment())
    "Assembly.cs".WriteFile("// " + Guid.NewGuid());

services.AddRazorPages();
services.AddServerSideBlazor();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();