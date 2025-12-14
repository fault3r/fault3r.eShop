
using System;
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
app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
