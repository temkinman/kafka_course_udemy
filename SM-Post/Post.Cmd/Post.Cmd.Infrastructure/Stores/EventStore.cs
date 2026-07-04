using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public EventStore(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }
    
    public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (expectedVersion != -1 && expectedVersion != eventStream[^1].Version)
        {
            throw new ConcurrencyException();
        }
        
        var version = expectedVersion;

        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                TimeStamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                EventType = eventType,
                EventData = @event,
                AggregateType = nameof(PostAggregate),
                Version = version
            };
            
            await _eventStoreRepository.SaveAsync(eventModel);
        }
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (eventStream == null || !eventStream.Any())
        {
            throw new AggregateException($"Incorrect post ID provided. No events found for aggregate id: {aggregateId}");
        }
        
        return eventStream
            .OrderBy(x => x.Version)
            .Select(x => x.EventData)
            .ToList();
    }
}