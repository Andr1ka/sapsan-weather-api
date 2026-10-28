using Sapsan.WeatherApi.Common;
using Sapsan.WeatherApi.Interfaces;
using Sapsan.WeatherApi.Models;
using Sapsan.WeatherApi.Repositories;
using Sapsan.WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherForecastRepository>(_ =>
    new JsonWeatherForecastRepository(Path.Combine(AppContext.BaseDirectory, "Data", "weather.json")));
builder.Services.AddSingleton<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/weather/forecast/{date}", (DateOnly date, IWeatherForecastService service) =>
{
    var result = service.GetByDate(date);
    return ToHttpResult(result);
});

app.MapPost("/weather/forecast", (WeatherForecast forecast, IWeatherForecastService service) =>
{
    var result = service.AddForecast(forecast);
    if (result is { IsSuccess: true, StatusCode: 201 })
    {
        return Results.Created($"/weather/forecast/{result.Value!.Date:yyyy-MM-dd}", result.Value);
    }

    return ToHttpResult(result);
});

app.Run();

static IResult ToHttpResult(Result<WeatherForecast> result)
{
    if (result.IsSuccess)
    {
        return Results.Ok(result.Value);
    }

    return Results.Json(result.Error, statusCode: result.StatusCode);
}
