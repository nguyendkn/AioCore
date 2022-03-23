using AioCore.Redis.OM.RedisCore;
using AioCore.Web.Domain;
using MediatR;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

if (environment.IsDevelopment())
    "Assembly.cs".WriteFile("// " + Guid.NewGuid());

var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddMediatR(typeof(Program).Assembly);
services.AddEndpointsApiExplorer();
services.AddControllers();
services.AddSwaggerGen();
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddRedisContext<AioCoreContext>(connectionString);
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