namespace BigDataApiAndIngestion.Services.IngestionService.Models
{
    public class StockTransaction
    {
        public Guid Id { get; set; }
        public string Symbol {  get; set; }
        public decimal Price { get; set; }
        public int Volume { get; set; }
    }
}
