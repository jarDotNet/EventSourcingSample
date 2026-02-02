using EventSourcingSample.ROP;
using EventSourcingSample.WebAPI.ROP;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static EventSourcingSample.WebAPI.Features.Orders.Application.CreateOrder;
using static EventSourcingSample.WebAPI.Features.Orders.Application.GetOrder;
using static EventSourcingSample.WebAPI.Features.Orders.Application.MarkOrderAsDelivered;
using static EventSourcingSample.WebAPI.Features.Orders.Application.MarkOrderAsDispatched;
using static EventSourcingSample.WebAPI.Features.Orders.Application.MarkOrderAsPaid;

namespace EventSourcingSample.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController
{
    private readonly ICreateOrderHandler _createOrderHandler;
    private readonly IGetOrderHandler _getOrderHandler;
    private readonly IMarkOrderAsPaidHandler _orderPaidHandler;
    private readonly IMarkOrderAsDispatchedHandler _orderDispatchedHandler;
    private readonly IMarkOrderAsDeliveredHandler _orderDeliveredHandler;


    public OrdersController(
        ICreateOrderHandler createOrderHandler,
        IGetOrderHandler getOrderHandler,
        IMarkOrderAsPaidHandler orderPaidHandler,
        IMarkOrderAsDispatchedHandler orderDispatchedHandler, 
        IMarkOrderAsDeliveredHandler orderDeliveredHandler)
    {
        _createOrderHandler = createOrderHandler;
        _getOrderHandler = getOrderHandler;
        _orderPaidHandler = orderPaidHandler;
        _orderDispatchedHandler = orderDispatchedHandler;
        _orderDeliveredHandler = orderDeliveredHandler;
    }

    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(ResultDto<GetOrderResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultDto<GetOrderResponse>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        return await _getOrderHandler.Handle(orderId)
            .UseSuccessHttpStatusCode(HttpStatusCode.OK)
            .ToActionResult();
    }

    [HttpPost("")]
    [ProducesResponseType(typeof(ResultDto<CreateOrderResponse>), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest createOrderRequest, CancellationToken cancellationToken = default)
    {
        return await _createOrderHandler.Handle(createOrderRequest, cancellationToken)
            .UseSuccessHttpStatusCode(HttpStatusCode.Created)
            .ToActionResult();
    }

    [HttpPut("{orderId:guid}/mark-as-paid")]
    [ProducesResponseType(typeof(ResultDto<Unit>), (int)HttpStatusCode.Accepted)]
    [ProducesResponseType(typeof(ResultDto<Unit>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> OrderPaid([FromRoute] Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _orderPaidHandler.Handle(orderId, cancellationToken)
            .UseSuccessHttpStatusCode(HttpStatusCode.Accepted)
            .ToActionResult();
    }

    [HttpPut("{orderId:guid}/mark-as-dispatched")]
    [ProducesResponseType(typeof(ResultDto<Unit>), (int)HttpStatusCode.Accepted)]
    [ProducesResponseType(typeof(ResultDto<Unit>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> OrderDispatched([FromRoute] Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _orderDispatchedHandler.Handle(orderId, cancellationToken)
            .UseSuccessHttpStatusCode(HttpStatusCode.Accepted)
            .ToActionResult();
    }

    [HttpPut("{orderId:guid}/mark-as-delivered")]
    [ProducesResponseType(typeof(ResultDto<Unit>), (int)HttpStatusCode.Accepted)]
    [ProducesResponseType(typeof(ResultDto<Unit>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> OrderDelivered([FromRoute] Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _orderDeliveredHandler.Handle(orderId, cancellationToken)
            .UseSuccessHttpStatusCode(HttpStatusCode.Accepted)
            .ToActionResult();
    }
}
