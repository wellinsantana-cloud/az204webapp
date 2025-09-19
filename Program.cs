var builder = WebApplication.CreateBuilder(args);

// Swagger siempre activo
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Activar Swagger en todos los entornos
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS y redirección
app.UseHttpsRedirection();

// Página HTML en la raíz
app.MapGet("/", async context =>
{
    var html = """
    <!DOCTYPE html>
    <html>
    <head>
        <title>API Az204</title>
    </head>
    <body>
        <h1>🚀 Bienvenido a la API Az204</h1>
        <p>Visita <a href="/swagger">Swagger UI</a> para explorar los endpoints.</p>
        <p>También puedes probar <a href="/weatherforecast">/weatherforecast</a> directamente.</p>
    </body>
    </html>
    """;
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

// Endpoint de ejemplo
var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )).ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}