namespace Sapsan.WeatherApi.Models;

public sealed class WeatherForecast
{
    public DateOnly Date { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? WindSpeed { get; set; }
}
