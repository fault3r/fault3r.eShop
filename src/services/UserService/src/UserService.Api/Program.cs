
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

builder.Host.AddSerilogLogging();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddInfrastructure();

builder.Services.AddUseCases();

builder.Services.AddPostgresDbContext(builder.Configuration);

builder.Services.AddControllers(config =>
    config.SuppressAsyncSuffixInActionNames = false);

builder.Services.AddFluentEmailService(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseCrossCuttingMiddleware(settings.CorrelationHeader);
app.UseExceptionHandlingMiddleware(settings.CorrelationHeader);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/", () => settings.Service);

app.Run();
