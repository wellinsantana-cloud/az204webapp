var builder = WebApplication.CreateBuilder(args);

// Agregar soporte para Swagger siempre
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Activar Swagger en todos los entornos
app.UseSwagger();
app.UseSwaggerUI();

// Redirigir la raíz "/" a Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/fail", () =>
{
    Console.WriteLine("⚠️ Generando error intencional...");
    throw new Exception("Error de prueba para logging");
});

// Endpoint de ejemplo
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}