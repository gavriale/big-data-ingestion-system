
using Confluent.Kafka;

namespace BigDataApiAndIngestion.Services.IngestionService.Strategies
{
    public interface IMessageProcessor<TValue>
    {
        Task ProcessAsync(Message<string, TValue> message);
    }
}
