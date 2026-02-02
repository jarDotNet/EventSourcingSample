using EventSourcingSample.EventSourcing;
using EventSourcingSample.EventSourcing.EventStores;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Aggregates;

namespace EventSourcingSample.WebAPI.Features.Orders.Infrastructure;

public class OrderRepository(IEventStore eventStore) : AggregateRepository<OrderDetails>(eventStore), IOrderRepository
{
    public async Task<OrderDetails?> GetById(Guid id, CancellationToken cancellationToken = default)
        => await GetByIdAsync(id, cancellationToken);

    public async Task Save(OrderDetails orderDetails, CancellationToken cancellationToken = default)
        => await SaveAsync(orderDetails, cancellationToken);
}
