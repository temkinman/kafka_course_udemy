using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;

namespace Post.Cmd.Infrastructure.Producers;

public class EventProducer : IEventProducer
{
    private readonly ProducerConfig _producerConfig;

    public EventProducer(IOptions<ProducerConfig> producerConfig)
    {
        _producerConfig = producerConfig.Value;
    }

    public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
    {
        using var producer = new ProducerBuilder<string, string>(_producerConfig)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();

        var eventMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event, @event.GetType())
        };
        
        var deliveryResult = producer.ProduceAsync(topic, eventMessage).Result;
        if (deliveryResult.Status != PersistenceStatus.Persisted)
        {
            throw new Exception($"Could not produce event: {@event.GetType().Name} message to topic - {topic} due to the following reason: {deliveryResult.Message}");
        }
        
        
    }
}