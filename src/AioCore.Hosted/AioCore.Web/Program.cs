var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddAntDesign();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();