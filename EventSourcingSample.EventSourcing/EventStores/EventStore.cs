namespace EventSourcingSample.EventSourcing.EventStores;

public interface IEventStore
{
    Task SaveEvents(string aggregateType, Guid aggregateId, IList<AggregateChange> events, int expectedVersion,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<AggregateChange>> GetEventsForAggregate(string aggregateType, Guid aggregateId,
        CancellationToken cancellationToken = default);
}

public class EventStore(IEventStoreManager eventStoreManager) : IEventStore
{
    private readonly IEventStoreManager _eventStoreManager = eventStoreManager;

    public async Task SaveEvents(string aggregateType, Guid aggregateId, IList<AggregateChange> events,
        int expectedVersion, CancellationToken cancellationToken = default)
    {
        await _eventStoreManager.SaveEvents(aggregateId, aggregateType, events, expectedVersion, cancellationToken);
    }

    public Task<IEnumerable<AggregateChange>> GetEventsForAggregate(string aggregateType, Guid aggregateId,
        CancellationToken cancellationToken = default)
    {
        return _eventStoreManager.GetEventsForAggregate(aggregateType, aggregateId, cancellationToken);
    }
}
