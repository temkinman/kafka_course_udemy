using CQRS.Core.Domain;

namespace CQRS.Core.Handlers;

public interface IEventSourcingHandler<T>
{
    Task SaveEventAsync(AggregateRoot aggregate);
    Task<T> GetByIdAsync(Guid aggregateId);
}