using System.Threading.Tasks;

namespace Producer.Kafka;

public interface IMessageProducer
{
    Task PublishAsync<T>(string topic, T message)
        where T : class;
}
