using BigDataApiAndIngestion.Services.IngestionService.Models;
using BigDataApiAndIngestion.Services.IngestionService.Strategies;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BigDataApiAndIngestion.Services.IngestionService.Consumers
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly KafkaConsumer<StockTransaction> _consumer;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(IMessageProcessor<StockTransaction> messageProcessor, ILogger<KafkaConsumerService> logger, ILogger<KafkaConsumer<StockTransaction>> consumerLogger)
        {
            _consumer = new KafkaConsumer<StockTransaction>(messageProcessor, consumerLogger);
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("KafkaConsumerService is starting.");
            return Task.Run(() => _consumer.Consume("stock-transactions", "localhost:9092", stoppingToken), stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("KafkaConsumerService is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
