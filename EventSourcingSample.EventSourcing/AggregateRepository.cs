using EventSourcingSample.EventSourcing.EventStores;
using System.Runtime.CompilerServices;

namespace EventSourcingSample.EventSourcing;

public interface IAggregateRepository<TAggregate>
{
    Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
}

public class AggregateRepository<TAggregate> : IAggregateRepository<TAggregate>
    where TAggregate : Aggregate
{
    private readonly IEventStore _eventStore;
    private static string AggregateName => typeof(TAggregate).Name;

    public AggregateRepository(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var events =
            (await _eventStore.GetEventsForAggregate(AggregateName, id, cancellationToken)).ToList();
        if (events.Count == 0)
        {
            return null;
        }

        var aggregate = (TAggregate)RuntimeHelpers.GetUninitializedObject(typeof(TAggregate));
        aggregate.Initialize(id);
        aggregate.LoadFromHistory(events);
        return aggregate;
    }

    public async Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var uncommittedEvents = aggregate.GetUncommittedChanges();
        if (uncommittedEvents.Count == 0)
        {
            return;
        }

        await _eventStore.SaveEvents(
            AggregateName,
            aggregate.Id,
            uncommittedEvents,
            aggregate.Version, cancellationToken);
        aggregate.MarkChangesAsCommitted();
    }
}
