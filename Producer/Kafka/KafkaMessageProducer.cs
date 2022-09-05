using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Producer.Kafka;

public class KafkaMessageProducer : IMessageProducer
{
    private readonly ILogger<KafkaMessageProducer> _logger;
    private readonly IProducer<Null, string> _producer;

    public KafkaMessageProducer(ILogger<KafkaMessageProducer> logger, IProducer<Null, string> producer)
    {
        _logger = logger;
        _producer = producer;
    }

    public async Task PublishAsync<T>(string topic, T message)
        where T : class
    {
        try
        {
            string jsonMessage = JsonSerializer.Serialize(message);
            Message<Null, string> kafkaMessage = new Message<Null, string>() { Value = jsonMessage };
            await _producer.ProduceAsync(topic, kafkaMessage);
            _logger.LogInformation($"The published message: {jsonMessage}");
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception, exception.Message);
        }
    }
}
