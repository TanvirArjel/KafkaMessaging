using Confluent.Kafka;

namespace Consumer.HostedServices;

public static class ServiceCollectionExtensions
{
    private static readonly ConsumerConfig KafkaConsumerConfig = new ConsumerConfig()
    {
        GroupId = "weather-update-group",
        BootstrapServers = "localhost:9092",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    public static void AddKafkaConsumer(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton(x => new ConsumerBuilder<Null, string>(KafkaConsumerConfig).Build());
    }
}
