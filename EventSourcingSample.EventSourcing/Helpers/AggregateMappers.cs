namespace EventSourcingSample.EventSourcing.Helpers;

internal static class AggregateMappers
{
    public static AggregateChange ToAggregateChange(AggregateChangeDto changeDto)
    {
        return new AggregateChange(
            changeDto.Content,
            changeDto.AggregateId,
            changeDto.GetType(),
            changeDto.TransactionId,
            changeDto.AggregateVersion,
            false
        );
    }

    public static AggregateChangeDto ToTypedAggregateChangeDto(
        Guid Id,
        string aggregateType,
        AggregateChange change
    )
    {
        return new AggregateChangeDto(
            change.Content,
            Id,
            aggregateType,
            change.TransactionId,
            change.Version
        );
    }
}
