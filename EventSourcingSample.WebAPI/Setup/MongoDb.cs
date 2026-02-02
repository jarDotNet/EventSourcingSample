using EventSourcingSample.MongoDB;

namespace EventSourcingSample.WebAPI.Setup;

public static class MongoDb
{
    public static IServiceCollection AddMongoDbConnectionProvider(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .AddMongoDbConnectionProvider()
            .AddMongoDbDatabaseConfiguration(configuration);
    }
}
