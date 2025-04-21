namespace EventSourcingSample.EventSourcing;

public record AggregateChange(object Content, Guid Id, Type Type, string TransactionId, int Version, bool IsNew);

//The dto is the one stored in the DB
internal class AggregateChangeDto
{
    public object Content { get; private set; }
    public Guid AggregateId { get; private set; }

    public string AggregateType { get; private set; }
    public string TransactionId { get; private set; }
    public int AggregateVersion { get; private set; }
    public DateTime Created { get; private set; }

    public AggregateChangeDto(object content, Guid aggregateId, string aggregateType, string transactionId, int aggregateVersion)
    {
        Content = content;
        AggregateId = aggregateId;
        AggregateType = aggregateType;
        TransactionId = transactionId;
        AggregateVersion = aggregateVersion;
        Created = DateTime.UtcNow;
    }
}
