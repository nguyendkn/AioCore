using MediatR;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddCors();
services.AddMemoryCache();
services.AddSwaggerGen();
services.AddEndpointsApiExplorer();
services.AddControllers();
services.AddHttpContextAccessor();
services.AddMediatR(typeof(Program).Assembly);
var app = builder.Build();
app.UseCors();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();