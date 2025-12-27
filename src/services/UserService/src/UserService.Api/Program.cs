
using System;
using UserService.Api.Middlewares;
using UserService.Infrastructure.DependencyInjection;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(AppSetting))
    .Get<AppSetting>()
        ?? throw new MissingAppSettingException();

builder.Services.AddInfrastructure();

builder.Services.AddUseCases();

builder.Host.AddSerilogLogging();

builder.Services.AddPostgresDbContext(builder.Configuration);

builder.Services.AddControllers(config =>
    config.SuppressAsyncSuffixInActionNames = false);

builder.Services.AddFluentEmailService(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var temp = builder.Environment;

var app = builder.Build();

app.UseCrossCuttingMiddleware(settings.CorrelationHeader);
app.UseExceptionHandlingMiddleware(settings.CorrelationHeader);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/", () => settings.Application);

app.Run();
