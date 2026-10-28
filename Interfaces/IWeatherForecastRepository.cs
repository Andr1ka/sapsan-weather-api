using Sapsan.WeatherApi.Models;

namespace Sapsan.WeatherApi.Interfaces;

public interface IWeatherForecastRepository
{
    List<WeatherForecast> Load();

    void Save(List<WeatherForecast> forecasts);
}
