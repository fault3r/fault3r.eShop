
using System;
using AccountService.Api.Configurations;
using AccountService.Application.Interfaces.Services;
using AccountService.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings))
    .Get<ApplicationSettings>() ??
    throw new NullReferenceException(nameof(ApplicationSettings));

builder.Services.AddSerilogConfiguration(appSettings.Log);

builder.Services.AddControllers(config =>
{
    config.SuppressAsyncSuffixInActionNames = true;
});

builder.Services.AddPostgreSqlContextConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () =>
    $"Name: {appSettings.Name}\n" +
    $"Description: {appSettings.Description}\n" +
    $"Version: {appSettings.Version}");

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerService<Program>>();
    await logger.LogInformation($"✅ {appSettings.Name} is running..");
}

app.Run();
