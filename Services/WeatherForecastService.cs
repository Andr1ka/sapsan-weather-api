using Sapsan.WeatherApi.Common;
using Sapsan.WeatherApi.Interfaces;
using Sapsan.WeatherApi.Models;

namespace Sapsan.WeatherApi.Services;

public sealed class WeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherForecastRepository _repository;

    public WeatherForecastService(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public Result<WeatherForecast> GetByDate(DateOnly date)
    {
        var forecasts = _repository.Load();
        var forecast = forecasts.FirstOrDefault(item => item.Date == date);

        if (forecast is null)
        {
            return Result<WeatherForecast>.Fail($"Прогноз на {date:yyyy-MM-dd} не найден.", 404);
        }

        return Result<WeatherForecast>.Ok(forecast);
    }

    public Result<WeatherForecast> AddForecast(WeatherForecast forecast)
    {
        var error = Validate(forecast);
        if (error is not null)
        {
            return Result<WeatherForecast>.Fail(error, 400);
        }

        var forecasts = _repository.Load();
        if (forecasts.Any(item => item.Date == forecast.Date))
        {
            return Result<WeatherForecast>.Fail($"Прогноз на {forecast.Date:yyyy-MM-dd} уже существует.", 409);
        }

        forecasts.Add(forecast);
        _repository.Save(forecasts);
        return Result<WeatherForecast>.Created(forecast);
    }

    private static string? Validate(WeatherForecast forecast)
    {
        if (forecast.Date == default)
        {
            return "Некорректная дата.";
        }

        if (forecast.Temperature is null)
        {
            return "Некорректная температура.";
        }

        if (forecast.WindSpeed is null || forecast.WindSpeed < 0)
        {
            return "Некорректная скорость ветра.";
        }

        return null;
    }
}
