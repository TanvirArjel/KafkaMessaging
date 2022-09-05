using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Producer;
using Producer.Kafka;

namespace Producer.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMessageProducer _messageProducer;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMessageProducer messageProducer)
    {
        _logger = logger;
        _messageProducer = messageProducer;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult> Post()
    {
        WeatherForecast weatherForecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };

        await _messageProducer.PublishAsync("weather-topic", weatherForecast);

        return Ok();
    }
}
