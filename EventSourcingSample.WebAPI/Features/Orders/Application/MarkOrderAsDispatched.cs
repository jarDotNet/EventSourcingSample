using EventSourcingSample.ROP;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Aggregates;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Events;

namespace EventSourcingSample.WebAPI.Features.Orders.Application;

public sealed class MarkOrderAsDispatched
{
    public interface IMarkOrderAsDispatchedHandler
    {
        Task<Result<Unit>> Handle(Guid orderId, CancellationToken cancellationToken = default);
    }

    public class MarkOrderAsDispatchedHandler : IMarkOrderAsDispatchedHandler
    {
        private readonly IOrderRepository _orderRepository;

        public MarkOrderAsDispatchedHandler(IOrderRepository orderRepository)
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
            if (orderDetails.Status != OrderStatus.Paid)
            {
                return Result.Failure($"Order {orderId} with status {orderDetails.Status} cannot be set as Dispatched");
            }

            orderDetails.Apply(new OrderDispatched());

            await _orderRepository.Save(orderDetails, cancellationToken);

            return Result.Success();
        }
    }
}
