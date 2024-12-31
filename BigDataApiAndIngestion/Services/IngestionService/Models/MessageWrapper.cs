namespace BigDataApiAndIngestion.Services.IngestionService.Models
{
    public class MessageWrapper<TPayload>
    {
        public string Type { get; set; }
        public TPayload Payload { get; set; }
    }

}
