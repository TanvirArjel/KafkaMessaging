using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace Consumer.HostedServices;

public class WeatherUpdateTopicListenerTwo : BackgroundService
{
    private readonly ILogger<WeatherUpdateTopicListenerTwo> _logger;
    private readonly IConsumer<Null, string> _consumer;

    public WeatherUpdateTopicListenerTwo(ILogger<WeatherUpdateTopicListenerTwo> logger, IConsumer<Null, string> consumer)
    {
        _logger = logger;
        _consumer = consumer;
        _consumer.Subscribe("weather-topic");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<Null, string> response = _consumer.Consume(stoppingToken);

                if (response.Message != null)
                {
                    WeatherForecast weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(response.Message.Value);
                    _logger.LogInformation($"The received message in service two is : {response.Message.Value}");
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception, exception.Message);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogCritical("WeatherUpdateListenerTwo has been stoped.");
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}
