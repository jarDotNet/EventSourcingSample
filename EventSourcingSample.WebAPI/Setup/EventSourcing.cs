namespace EventSourcingSample.WebAPI.Setup;

public static class EventSourcing
{
    public static void AddEventSourcing(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddMongoEventSourcing(configuration);
    }
}
