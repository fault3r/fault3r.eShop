
using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using UserService.Api.Middlewares;
using UserService.Infrastructure.DependencyInjection;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(AppSettings))
    .Get<AppSettings>()
        ?? throw new MissingAppSettingsException();

builder.WebHost.UseUrls(settings.Urls.Http);

builder.Host.AddSerilogLogging();

builder.Services.AddRateLimiting(builder.Configuration);

builder.Services.AddInfrastructure();

builder.Services.AddUseCases();

builder.Services.AddPostgresDbContext(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddRedisCaching(builder.Configuration);

builder.Services.AddApplicationSession(builder.Configuration);

builder.Services.AddApiVersioning(settings);

builder.Services.AddControllers(config =>
    config.SuppressAsyncSuffixInActionNames = false);

builder.Services.AddFluentEmailService(builder.Configuration);

#region ⤚Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "User Service API",
        Version = "v1",
        Description = "User Service API",
    });
});
builder.Services.AddVersionedApiExplorer(config =>
{
    config.GroupNameFormat = $"'v'V";
    config.SubstituteApiVersionInUrl = true;
});
#endregion

var app = builder.Build();

#region ⤚Swagger
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                url: $"/swagger/{description.GroupName}/swagger.json",
                name: description.GroupName.ToUpperInvariant()
            );
        }
    });
}
#endregion

app.UseRateLimiter();

app.UseCorrelationMiddleware(settings.CorrelationHeader);

app.UseExceptionHandlingMiddleware(settings.CorrelationHeader);

app.UseAuthentication();
app.UseAuthenticationMiddleware();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => settings.Service);

app.Run();
