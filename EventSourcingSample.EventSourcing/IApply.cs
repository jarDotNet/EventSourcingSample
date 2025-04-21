namespace EventSourcingSample.EventSourcing;

public interface IApply<TEvent>
{
    void Apply(TEvent ev);
}
