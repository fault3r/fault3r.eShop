
using AccountService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:7072", "http://localhost:5299");

string connectionString =
    $"Host=localhost;" +
    $"Port=5432;" +
    $"Username=eShop;" +
    $"Password=fault3r;" +
    $"Database=AccountsDatabase;";
builder.Services.AddDbContext<AccountDbContext>(config =>
{
    config.UseNpgsql(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "AccountService");

app.Run();
