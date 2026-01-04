
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

// builder.WebHost.UseUrls(settings.Urls.Http, settings.Urls.Https);

builder.Host.AddSerilogLogging();

builder.Services.AddInfrastructure();

builder.Services.AddUseCases();

builder.Services.AddPostgresDbContext(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddRedisCaching(builder.Configuration);

builder.Services.AddControllers(config =>
    config.SuppressAsyncSuffixInActionNames = false);

builder.Services.AddFluentEmailService(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCrossCuttingMiddleware(settings.CorrelationHeader);

app.UseExceptionHandlingMiddleware(settings.CorrelationHeader);

app.UseAuthentication();
app.UseAuthenticationMiddleware(); 
app.UseAuthorization();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/", () => settings.Service);

app.Run();
