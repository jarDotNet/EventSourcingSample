using EventSourcingSample.ROP;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Aggregates;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Events;

namespace EventSourcingSample.WebAPI.Features.Orders.Application;

public sealed class MarkOrderAsDelivered
{
    public interface IMarkOrderAsDeliveredHandler
    {
        Task<Result<Unit>> Handle(Guid orderId, CancellationToken cancellationToken = default);
    }

    public class MarkOrderAsDeliveredHandler : IMarkOrderAsDeliveredHandler
    {
        private readonly IOrderRepository _orderRepository;

        public MarkOrderAsDeliveredHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<Unit>> Handle(Guid orderId, CancellationToken cancellationToken = default)
        {
            var orderDetails = await _orderRepository.GetById(orderId, cancellationToken);
            if (orderDetails is null)
            {
                return Result.NotFound($"Order {orderId} not found");
            }
            if (orderDetails.Status != OrderStatus.Dispatched)
            {
                return Result.Failure($"Order {orderId} with status {orderDetails.Status} cannot be set as Delivered");
            }

            orderDetails.Apply(new OrderCompleted());

            await _orderRepository.Save(orderDetails, cancellationToken);

            return Result.Success();
        }
    }
}
