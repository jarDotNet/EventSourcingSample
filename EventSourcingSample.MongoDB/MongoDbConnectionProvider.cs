using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace EventSourcingSample.MongoDB;

public interface IMongoDbConnectionProvider
{
    MongoUrl GetMongoUrl();
    string GetMongoConnectionString();
}

public class MongoDbConnectionProvider : IMongoDbConnectionProvider
{
    private readonly IConfiguration _config;

    private MongoUrl? MongoUrl { get; set; }
    private string? MongoConnectionString { get; set; }

    public MongoDbConnectionProvider(IConfiguration config)
    {
        _config = config;
    }

    public MongoUrl GetMongoUrl()
    {
        if (MongoUrl is not null)
        {
            return MongoUrl;
        }

        MongoConnectionString = RetrieveMongoUrl();
        MongoUrl = new MongoUrl(MongoConnectionString);

        return MongoUrl;
    }

    public string GetMongoConnectionString()
    {
        if (MongoConnectionString is null)
        {
            GetMongoUrl();
        }

        return MongoConnectionString ?? throw new Exception("Mongo connection string cannot be retrieved");
    }

    private string? RetrieveMongoUrl()
    {
        var connectionString = _config["MongoDb:ConnectionString"];
        return connectionString;
    }
}
