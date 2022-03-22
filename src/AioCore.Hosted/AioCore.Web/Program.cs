using AioCore.Redis.OM;
using AioCore.Web.Domain;
using AioCore.Web.Domain.AggregateModels.PageAggregate;
using MediatR;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

if (environment.IsDevelopment())
    "Assembly.cs".WriteFile("// " + Guid.NewGuid());

var connectionString = configuration.GetConnectionString("DefaultConnection");
var redisProvider = new RedisConnectionProvider(connectionString);
redisProvider.Connection.CreateIndex(typeof(Page));

services.AddMediatR(typeof(Program).Assembly);
services.AddEndpointsApiExplorer();
services.AddControllers();
services.AddSwaggerGen();
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddSingleton(redisProvider);
services.AddSingleton(_ => new AioCoreContext(redisProvider));
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