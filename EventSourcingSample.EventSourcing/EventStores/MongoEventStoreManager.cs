﻿using EventSourcingSample.EventSourcing.Helpers;
using EventSourcingSample.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Data;

namespace EventSourcingSample.EventSourcing.EventStores;

public class MongoEventStoreManager : IEventStoreManager
{
    private readonly IMongoDatabase _mongoDatabase;
    private readonly MongoEventStoreConfiguration _mongoDbMongoEventStoreConfiguration;

    private IMongoCollection<AggregateChangeDto> _eventStoreChangesCollection =>
        _mongoDatabase.GetCollection<AggregateChangeDto>(_mongoDbMongoEventStoreConfiguration.CollectionName);

    public MongoEventStoreManager(IMongoDbConnectionProvider mongoDbConnectionProvider, IOptions<MongoEventStoreConfiguration> mongoDbEventStoreOptions)
    {
        var objectSerializer = new ObjectSerializer(x => true);
        BsonSerializer.TryRegisterSerializer(objectSerializer);
        BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        _mongoDbMongoEventStoreConfiguration = mongoDbEventStoreOptions.Value;
        
        var mongoClient = new MongoClient(mongoDbConnectionProvider.GetMongoUrl());
        _mongoDatabase = mongoClient.GetDatabase(_mongoDbMongoEventStoreConfiguration.DatabaseName);
    }

    public async Task SaveEvents(Guid id, string aggregateType, IEnumerable<AggregateChange> events, int expectedVersion, CancellationToken cancellationToken = default)
    {
        var collection = _eventStoreChangesCollection;
        await CreateIndex(collection);
        var latestAggregate = await collection
            .Find(d => d.AggregateType == aggregateType && d.AggregateId == id)
            .SortByDescending(d => d.AggregateVersion)
            .Limit(1)
            .FirstOrDefaultAsync(cancellationToken);
        var latestAggregateVersion = latestAggregate?.AggregateVersion;

        if (latestAggregateVersion.HasValue && latestAggregateVersion >= expectedVersion)
        {
            throw new DBConcurrencyException("Concurrency exception");
        }

        var dtos = events.Select(e =>
            AggregateMappers.ToTypedAggregateChangeDto(id, aggregateType, e)
        );

        await collection.InsertManyAsync(dtos, new InsertManyOptions() { IsOrdered = true }, cancellationToken);
    }

    public async Task<IEnumerable<AggregateChange>> GetEventsForAggregate(string aggregateType, Guid id, CancellationToken cancellationToken = default)
    {
        List<AggregateChangeDto>? result = await _eventStoreChangesCollection
            .Find(aggregate => aggregate.AggregateType == aggregateType && aggregate.AggregateId == id)
            .SortBy(a => a.AggregateVersion)
            .ToListAsync(cancellationToken);

        return result.Select(AggregateMappers.ToAggregateChange);
    }

    private static async Task CreateIndex(IMongoCollection<AggregateChangeDto> collection)
    {
        await collection.Indexes.CreateOneAsync(new CreateIndexModel<AggregateChangeDto>(
                Builders<AggregateChangeDto>.IndexKeys
                    .Ascending(i => i.AggregateType)
                    .Ascending(i => i.AggregateId)
                    .Ascending(i => i.AggregateVersion),
                new CreateIndexOptions { Unique = true, Name = "_Aggregate_Type_Id_Version_" }))
            .ConfigureAwait(false);
    }
}
