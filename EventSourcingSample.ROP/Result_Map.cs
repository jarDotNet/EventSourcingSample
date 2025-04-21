using System.Runtime.ExceptionServices;

namespace EventSourcingSample.ROP;

/// <summary>
/// Provides extension methods for mapping results from one type to another.
/// </summary>
public static class Result_Map
{
    public static Result<U> Map<T, U>(this Result<T> r, Func<T, U> mapper)
    {
        try
        {
            return r.Success
                ? Result.Success(mapper(r.Value))
                : Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<Result<U>> Map<T, U>(this Task<Result<T>> result, Func<T, U> mapper)
    {
        try
        {
            var r = await result;
            return r.Success
                ? Result.Success(mapper(r.Value))
                : Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<Result<U>> Map<T, U>(this Task<Result<T>> result, Func<T, Task<U>> mapper)
    {
        try
        {
            var r = await result;
            return r.Success
                ? Result.Success(await mapper(r.Value))
                : Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}
