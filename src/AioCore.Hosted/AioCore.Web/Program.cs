var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddRazorPages();
services.AddServerSideBlazor();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();