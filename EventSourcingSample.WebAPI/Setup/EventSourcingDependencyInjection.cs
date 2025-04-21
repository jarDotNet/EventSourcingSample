using EventSourcingSample.EventSourcing;
using EventSourcingSample.EventSourcing.EventStores;
using EventSourcingSample.MongoDB;

namespace EventSourcingSample.WebAPI.Setup;

public static class EventSourcingDependencyInjection
{
    public static void AddMongoEventSourcing(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient(typeof(IAggregateRepository<>), typeof(AggregateRepository<>));
        serviceCollection.AddTransient<IEventStore, EventStore>();
        serviceCollection.AddTransient<IEventStoreManager, MongoEventStoreManager>();
        serviceCollection.Configure<MongoEventStoreConfiguration>(configuration.GetSection("EventSourcing"));
    }
}