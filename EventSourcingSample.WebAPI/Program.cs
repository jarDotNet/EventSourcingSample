using EventSourcingSample.WebAPI.Features.Orders.Domain.Aggregates;
using EventSourcingSample.WebAPI.Features.Orders.Infrastructure;
using EventSourcingSample.WebAPI.Setup;
using static EventSourcingSample.WebAPI.Features.Orders.Application.CreateOrder;
using static EventSourcingSample.WebAPI.Features.Orders.Application.GetOrder;
using static EventSourcingSample.WebAPI.Features.Orders.Application.MarkOrderAsDelivered;
using static EventSourcingSample.WebAPI.Features.Orders.Application.MarkOrderAsDispatched;
using static EventSourcingSample.WebAPI.Features.Orders.Application.MarkOrderAsPaid;

WebApplication app = DefaultDistribtWebApplication.Create(args, webappBuilder =>
{
    MongoEventMappings.RegisterClasses();
    webappBuilder.Services.AddMongoDbConnectionProvider(webappBuilder.Configuration);
    webappBuilder.Services.AddEventSourcing(webappBuilder.Configuration);
    webappBuilder.Services.AddScoped<IOrderRepository, OrderRepository>();
    webappBuilder.Services.AddScoped<ICreateOrderHandler, CreateOrderHandler>();
    webappBuilder.Services.AddScoped<IGetOrderHandler, GetOrderHandler>();
    webappBuilder.Services.AddScoped<IMarkOrderAsPaidHandler, MarkOrderAsPaidHandler>();
    webappBuilder.Services.AddScoped<IMarkOrderAsDispatchedHandler, MarkOrderAsDispatchedHandler>();
    webappBuilder.Services.AddScoped<IMarkOrderAsDeliveredHandler, MarkOrderAsDeliveredHandler>();
});


DefaultDistribtWebApplication.Run(app);
