
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config =>
{
    config.SuppressAsyncSuffixInActionNames = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapControllers();

app.MapGet("/", () => "AccountService");

app.Run();
