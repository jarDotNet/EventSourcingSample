using EventSourcingSample.WebAPI.Features.Orders.Application.Shared;

namespace EventSourcingSample.WebAPI.Features.Orders.Domain.Events;

public record OrderCreated(DeliveryDetails Delivery, PaymentInformation PaymentInformation, IEnumerable<ProductQuantity> Products);

public record OrderPaid();

public record OrderDispatched();

public record OrderCompleted();

public enum OrderStatus
{
    Created,
    Paid,
    Dispatched,
    Completed,
    Failed
}