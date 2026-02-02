using EventSourcingSample.WebAPI.Features.Orders.Domain.Events;
using MongoDB.Bson.Serialization;

namespace EventSourcingSample.WebAPI.Setup;

public static class MongoEventMappings
{
    public static void RegisterClasses()
    {
        BsonClassMap.RegisterClassMap<OrderCreated>();
        BsonClassMap.RegisterClassMap<OrderPaid>();
        BsonClassMap.RegisterClassMap<OrderDispatched>();
        BsonClassMap.RegisterClassMap<OrderCompleted>();
    }
}
