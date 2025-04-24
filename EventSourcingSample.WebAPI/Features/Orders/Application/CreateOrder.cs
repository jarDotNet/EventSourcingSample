using EventSourcingSample.ROP;
using EventSourcingSample.WebAPI.Features.Orders.Application.Shared;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Aggregates;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Events;

namespace EventSourcingSample.WebAPI.Features.Orders.Application;

public sealed class CreateOrder
{
    public record CreateOrderRequest(DeliveryDetails DeliveryDetails, PaymentInformation PaymentInformation, IEnumerable<ProductQuantity> Products);

    public record CreateOrderResponse(Guid OrderId, string Location);

    public interface ICreateOrderHandler
    {
        Task<Result<CreateOrderResponse>> Handle(CreateOrderRequest createOrder, CancellationToken cancellationToken = default);
    }

    internal class CreateOrderHandler : ICreateOrderHandler
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<CreateOrderResponse>> Handle(CreateOrderRequest createOrder, CancellationToken cancellationToken = default)
        {
            return await CreateOrder(createOrder)
                .Async()
                .Bind(x => SaveOrder(x, cancellationToken))
                .Map(x => new CreateOrderResponse(x.Id, $"orders/{x.Id}"));
        }

        private static Result<OrderDetails> CreateOrder(CreateOrderRequest createOrder)
        {
            Guid createdOrderId = Guid.NewGuid();

            var orderDetails = new OrderDetails(createdOrderId);
            orderDetails.Apply(new OrderCreated(createOrder.DeliveryDetails, createOrder.PaymentInformation, createOrder.Products));

            return orderDetails;
        }

        private async Task<Result<OrderDetails>> SaveOrder(OrderDetails orderDetails, CancellationToken cancellationToken)
        {
            await _orderRepository.Save(orderDetails, cancellationToken);
            return orderDetails;
        }
    }
}
