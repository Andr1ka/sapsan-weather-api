using Sapsan.WeatherApi.Common;
using Sapsan.WeatherApi.Models;

namespace Sapsan.WeatherApi.Interfaces;

public interface IWeatherForecastService
{
    Result<WeatherForecast> GetByDate(DateOnly date);

    Result<WeatherForecast> AddForecast(WeatherForecast forecast);
}
