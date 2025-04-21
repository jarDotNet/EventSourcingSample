using EventSourcingSample.EventSourcing;
using EventSourcingSample.WebAPI.Features.Orders.Application.Shared;
using EventSourcingSample.WebAPI.Features.Orders.Domain.Events;
using System.Collections.Immutable;

namespace EventSourcingSample.WebAPI.Features.Orders.Domain.Aggregates;

public class OrderDetails : Aggregate, IApply<OrderCreated>, IApply<OrderPaid>, IApply<OrderDispatched>, IApply<OrderCompleted>
{
    public DeliveryDetails Delivery { get; private set; } = default!;
    public PaymentInformation PaymentInformation { get; private set; } = default!;
    public ImmutableArray<ProductQuantity> Products { get; private set; } = [];
    public OrderStatus Status { get; private set; }

    public OrderDetails(Guid id) : base(id)
    {
    }

    public void Apply(OrderCreated ev)
    {
        Delivery = ev.Delivery;
        PaymentInformation = ev.PaymentInformation;
        Products = [.. ev.Products];
        Status = OrderStatus.Created;
        ApplyChange(ev);
    }

    public void Apply(OrderPaid ev)
    {
        Status = OrderStatus.Paid;
        ApplyChange(ev);
    }

    public void Apply(OrderDispatched ev)
    {
        Status = OrderStatus.Dispatched;
        ApplyChange(ev);
    }

    public void Apply(OrderCompleted ev)
    {
        Status = OrderStatus.Completed;
        ApplyChange(ev);
    }
}
