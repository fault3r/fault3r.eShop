

using Serilog;
using UserService.Api.Middlewares;
using UserService.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogLogging();

builder.Services.AddServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCorrelationIdMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/",() => 
{ 
    Log.Information("request received.");
    return "User Service";
});



app.Run();
