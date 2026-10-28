using System.Text.Json;
using Sapsan.WeatherApi.Interfaces;
using Sapsan.WeatherApi.Models;

namespace Sapsan.WeatherApi.Repositories;

public sealed class JsonWeatherForecastRepository : IWeatherForecastRepository
{
    private readonly string _path;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public JsonWeatherForecastRepository(string path)
    {
        _path = path;
    }

    public List<WeatherForecast> Load()
    {
        if (!File.Exists(_path))
        {
            var empty = new List<WeatherForecast>();
            Save(empty);
            return empty;
        }

        var json = File.ReadAllText(_path);
        var data = JsonSerializer.Deserialize<List<WeatherForecast>>(json, _jsonOptions);
        return data ?? [];
    }

    public void Save(List<WeatherForecast> forecasts)
    {
        var directory = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(forecasts, _jsonOptions);
        File.WriteAllText(_path, json);
    }
}
