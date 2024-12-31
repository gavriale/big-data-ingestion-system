
using System;
using System.Text.Json;
using Confluent.Kafka;

namespace BigDataApiAndIngestion.Services.IngestionService.Desirializers
{
    public class CustomJsonDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                throw new ArgumentNullException(nameof(data), "Message data is null.");
            }

            try
            {
                // Deserialize JSON into the specified type
                return JsonSerializer.Deserialize<T>(data);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Error deserializing JSON message.", ex);
            }
        }
    }
}
