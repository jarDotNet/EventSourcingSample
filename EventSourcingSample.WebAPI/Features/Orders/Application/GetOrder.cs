using EventSourcingSample.ROP;
using EventSourcingSample.WebAPI.Features.Orders.Application.Shared;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Aggregates;

namespace EventSourcingSample.WebAPI.Features.Orders.Application;

public sealed class GetOrder
{
    public record GetOrderResponse(
        Guid OrderId, string OrderStatus, DeliveryDetails DeliveryDetails, 
        PaymentInformation PaymentInformation, IEnumerable<ProductQuantityName> Products
    );

    public interface IGetOrderHandler
    {
        Task<Result<GetOrderResponse>> Handle(Guid orderId, CancellationToken cancellationToken = default);
    }

    public class GetOrderHandler : IGetOrderHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<GetOrderResponse>> Handle(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await GetOrderDetails(orderId, cancellationToken)
                 .Bind(MapToOrderResponse);
        }

        private async Task<Result<OrderDetails>> GetOrderDetails(Guid orderId, CancellationToken cancellationToken = default)
        {
            OrderDetails? orderDetails = await _orderRepository.GetById(orderId, cancellationToken);
            if (orderDetails is null)
            {
                return Result.NotFound<OrderDetails>($"Order {orderId} not found");
            }

            return orderDetails;
        }

        private static Result<GetOrderResponse> MapToOrderResponse(OrderDetails orderDetails)
        {
            var products = orderDetails.Products
                .Select(p => new ProductQuantityName(p.ProductId, p.Quantity, $"Product {p.ProductId}"));

            return new GetOrderResponse(orderDetails.Id, orderDetails.Status.ToString(), orderDetails.Delivery,
                orderDetails.PaymentInformation, products);
        }
    }
}
