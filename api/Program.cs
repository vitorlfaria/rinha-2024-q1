var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/healthcheck", () =>
{
    return "Hello World!";
});

app.Run();
