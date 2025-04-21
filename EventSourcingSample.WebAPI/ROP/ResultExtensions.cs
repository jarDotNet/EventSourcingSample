using EventSourcingSample.ROP;

namespace EventSourcingSample.WebAPI.ROP;

internal static class ResultExtensions
{
    public static ResultDto<T> ToDto<T>(this Result<T> result)
    {
        return result.Success
            ? new ResultDto<T>()
            {
                Value = result.Value,
                Errors = []
            }
            : new ResultDto<T>()
            {
                Value = default!,
                Errors = [.. result.Errors.Select(x => new ErrorDto()
                {
                    ErrorCode = x.ErrorCode,
                    Message = x.Message
                })]
            };
    }
}
