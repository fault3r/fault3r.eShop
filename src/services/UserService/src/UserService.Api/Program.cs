
using System;
using UserService.Api.Middlewares;
using UserService.Infrastructure.DependencyInjection;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(AppSettings))
    .Get<AppSettings>()
        ?? throw new MissingAppSettingsException();

builder.Services.AddInfrastructure();

builder.Host.AddSerilogLogging();

builder.Services.AddPostgresDbContext(builder.Configuration);

builder.Services.AddControllers(config =>
    config.SuppressAsyncSuffixInActionNames = false);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCrossCuttingMiddleware(settings.Common.CorrelationHeader);
app.UseExceptionHandlingMiddleware(settings.Common.CorrelationHeader);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/",() => settings);

app.Run();
