using AioCore.Redis.OM;
using AioCore.Web.Domain;
using MediatR;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var environment = builder.Environment;
var configuration = builder.Configuration;

if (environment.IsDevelopment())
    "Assembly.cs".WriteFile("// " + Guid.NewGuid());

services.AddMediatR(typeof(Program).Assembly);
services.AddEndpointsApiExplorer();
services.AddControllers();
services.AddSwaggerGen();
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddRedisContext<AioCoreContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.ConnectionString = connectionString;
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();