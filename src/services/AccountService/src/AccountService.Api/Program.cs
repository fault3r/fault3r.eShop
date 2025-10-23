
using AccountService.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/", () => "AccountService");

app.Run();
