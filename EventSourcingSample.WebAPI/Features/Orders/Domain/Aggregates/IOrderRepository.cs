namespace EventSourcingSample.WebAPI.Features.Orders.Domain.Aggregates;

public interface IOrderRepository
{
    Task<OrderDetails?> GetById(Guid id, CancellationToken cancellationToken = default);

    Task Save(OrderDetails orderDetails, CancellationToken cancellationToken = default);
}