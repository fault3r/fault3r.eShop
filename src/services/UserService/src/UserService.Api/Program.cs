
using System;
using Serilog;
using UserService.Api.Middlewares;
using UserService.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Host.AddSerilogLogging();

builder.Host.AddPostgresDbContext();

builder.Services.AddControllers(config =>
{
    config.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddDIs();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCorrelationIdMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/",() => 
{ 
    Log.Information("request received.");
    return "User Service";
});

Console.WriteLine("app started.");

app.Run();
