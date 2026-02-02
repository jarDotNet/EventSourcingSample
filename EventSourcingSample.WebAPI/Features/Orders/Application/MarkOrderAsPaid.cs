using EventSourcingSample.ROP;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Aggregates;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Events;

namespace EventSourcingSample.WebAPI.Features.Orders.Application;

public sealed class MarkOrderAsPaid
{
    public interface IMarkOrderAsPaidHandler
    {
        Task<Result<Unit>> Handle(Guid orderId, CancellationToken cancellationToken = default);
    }

    public class MarkOrderAsPaidHandler : IMarkOrderAsPaidHandler
    {
        private readonly IOrderRepository _orderRepository;

        public MarkOrderAsPaidHandler(IOrderRepository orderRepository)
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
            if (orderDetails.Status != OrderStatus.Created)
            {
                return Result.Failure($"Order {orderId} with status {orderDetails.Status} cannot be set as Paid");
            }

            orderDetails.Apply(new OrderPaid());

            await _orderRepository.Save(orderDetails, cancellationToken);

            return Result.Success();
        }
    }
}
