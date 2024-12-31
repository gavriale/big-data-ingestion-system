using BigDataApiAndIngestion.Services.IngestionService.Desirializers;
using BigDataApiAndIngestion.Services.IngestionService.Models;
using BigDataApiAndIngestion.Services.IngestionService.Strategies;
using Confluent.Kafka;

public class KafkaConsumer<TValue> where TValue : class
{
    private readonly IMessageProcessor<TValue> _messageProcessor;
    private readonly ILogger<KafkaConsumer<TValue>> _logger;

    public KafkaConsumer(IMessageProcessor<TValue> messageProcessor, ILogger<KafkaConsumer<TValue>> logger)
    {
        _messageProcessor = messageProcessor;
        _logger = logger;
    }

    public async void Consume(string topic, string bootstrapServers, CancellationToken stoppingToken)
    {
        ConsumerConfig config = new ConsumerConfig
        {
            GroupId = "stock-transaction-group",
            BootstrapServers = bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using (var consumer = new ConsumerBuilder<string, MessageWrapper<TValue>>(config)
                                .SetValueDeserializer(new CustomJsonDeserializer<MessageWrapper<TValue>>()) // Use custom deserializer
                                .Build())
        {
            consumer.Subscribe(topic);

            try
            {
                _logger.LogInformation("Subscribed to topic {Topic}.", topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        var wrapper = result.Message.Value;

                        // Extract the Payload (TValue) from MessageWrapper
                        var payload = wrapper.Payload;

                        _logger.LogInformation("Consumed payload: {@Payload}", payload);

                        try
                        {
                            await _messageProcessor.ProcessAsync(new Message<string, TValue>
                            {
                                Key = result.Message.Key,
                                Value = payload
                            });

                            // Commit offsets after successful processing
                            consumer.Commit(result);
                            _logger.LogInformation("Committed offset for message: {Offset}", result.Offset);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing message.");
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError(e, "Error consuming message.");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Unexpected error during consumption.");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumer is shutting down gracefully.");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
