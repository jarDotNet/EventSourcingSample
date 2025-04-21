using System.Collections.Immutable;

namespace EventSourcingSample.WebAPI.ROP;

public class ResultDto<T>
{
    public T? Value { get; set; }
    public ImmutableArray<ErrorDto> Errors { get; set; }
    public bool Success => Errors.Length == 0;
}
