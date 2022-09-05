using Confluent.Kafka;

namespace Producer.Kafka;

public static class ServiceCollectionExtensions
{
    private static readonly ProducerConfig kafkaProducerConfig = new ProducerConfig()
    {
        BootstrapServers = "localhost:9092"
    };

    public static void AddKafkaProducer(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton(x => new ProducerBuilder<Null, string>(kafkaProducerConfig).Build());
    }
}
