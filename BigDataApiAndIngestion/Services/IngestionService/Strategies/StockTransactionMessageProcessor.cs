using BigDataApiAndIngestion.Services.IngestionService.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace BigDataApiAndIngestion.Services.IngestionService.Strategies
{
    public class StockTransactionMessageProcessor : IMessageProcessor<StockTransaction>
    {
        private readonly ILogger<StockTransactionMessageProcessor> _logger;

        public StockTransactionMessageProcessor(ILogger<StockTransactionMessageProcessor> logger)
        {
            _logger = logger;
        }

        public Task ProcessAsync(Message<string, StockTransaction> message)
        {
            // Log the consumed StockTransaction
            Console.WriteLine($"Processed StockTransaction: Id={message.Value.Id}, Symbol={message.Value.Symbol}, Price={message.Value.Price}, Volume={message.Value.Volume}");
            return Task.CompletedTask;
        }

        //private async Task SaveTransactionAsync(StockTransaction transaction)
        //{
        //    _logger.LogDebug($"Saving transaction: Id={transaction.Id}, Price={transaction.Price}, Volume={transaction.Volume}");

        //    try
        //    {
        //        // Simulate database save with delay
        //        await Task.Delay(100); // Simulating async database save
        //        _logger.LogInformation($"Transaction saved: Id={transaction.Id}");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error saving transaction: Id={transaction.Id}");
        //        throw;
        //    }
        //}
    }


}
