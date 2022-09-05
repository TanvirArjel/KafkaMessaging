using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace Consumer.HostedServices;

internal class MessageConsumer : BackgroundService
{
    private readonly ILogger<MessageConsumer> _log;
    private readonly IConsumer<Null, string> _consumer;

    public MessageConsumer(ILogger<MessageConsumer> log, IConsumer<Null, string> consumer)
    {
        _log = log;
        _consumer = consumer;
        consumer.Subscribe("weather-topic");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        int i = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            ConsumeResult<Null, string> consumeResult = _consumer.Consume(stoppingToken);

            _log.LogInformation(consumeResult.Message.Key + " - " + consumeResult.Message.Value);

            if (i++ % 1000 == 0)
            {
                _consumer.Commit();
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}
