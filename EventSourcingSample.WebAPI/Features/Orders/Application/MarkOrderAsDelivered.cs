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

    internal class MarkOrderAsDeliveredHandler : IMarkOrderAsDeliveredHandler
    {
        private readonly IOrderRepository _orderRepository;

        public MarkOrderAsDeliveredHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<Unit>> Handle(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await GetOrderDetails(orderId, cancellationToken)
                .Bind(ApplyOrderCompletedEvent)
                .Bind(x => SaveOrder(x, cancellationToken));
        }

        private async Task<Result<OrderDetails>> GetOrderDetails(Guid orderId, CancellationToken cancellationToken = default)
        {
            var orderDetails = await _orderRepository.GetById(orderId, cancellationToken);
            if (orderDetails is null)
            {
                return Result.NotFound<OrderDetails>($"Order {orderId} not found");
            }
            if (orderDetails.Status != OrderStatus.Dispatched)
            {
                return Result.Failure<OrderDetails>($"Order {orderId} with status {orderDetails.Status} cannot be set as Delivered");
            }

            return orderDetails;
        }

        private static Result<OrderDetails> ApplyOrderCompletedEvent(OrderDetails orderDetails)
        {
            orderDetails.Apply(new OrderCompleted());

            return orderDetails;
        }

        private async Task<Result<Unit>> SaveOrder(OrderDetails orderDetails, CancellationToken cancellationToken = default)
        {
            await _orderRepository.Save(orderDetails, cancellationToken);

            return Result.Success();
        }
    }
}
