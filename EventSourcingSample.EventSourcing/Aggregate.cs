namespace EventSourcingSample.EventSourcing;

public abstract class Aggregate
{
    private List<AggregateChange> _changes = [];

    public Guid Id { get; internal set; }
    public int Version { get; set; } = 0;

    /// <summary>
    /// This flag is used to identify when an event is being loaded from the DB
    /// or when the event is being created as new
    /// </summary>
    private bool ReadingFromHistory { get; set; } = false;

    protected Aggregate(Guid id)
    {
        Id = id;
    }

    internal void Initialize(Guid id)
    {
        Id = id;
        _changes = [];
    }

    public IList<AggregateChange> GetUncommittedChanges()
    {
        return [.. _changes.Where(a => a.IsNew)];
    }

    public void MarkChangesAsCommitted()
    {
        _changes.Clear();
    }

    protected void ApplyChange<T>(T eventObject)
    {
        ArgumentNullException.ThrowIfNull(eventObject, nameof(eventObject));

        Version++;

        AggregateChange change = new AggregateChange(
            eventObject,
            Id,
            eventObject.GetType(),
            $"{Id}:{Version}",
            Version,
            ReadingFromHistory != true
        );
        _changes.Add(change);
    }

    public void LoadFromHistory(IList<AggregateChange> history)
    {
        if (!history.Any())
        {
            return;
        }

        ReadingFromHistory = true;
        foreach (var e in history)
        {
            ApplyChanges(e.Content);
        }
        ReadingFromHistory = false;

        Version = history.Last().Version;

        void ApplyChanges<TEvent>(TEvent eventObject)
        {
            if (eventObject is not null)
            {
                dynamic me = this;
                dynamic eventObj = eventObject;
                me.Apply(eventObj);
            }   
        }
    }
}
