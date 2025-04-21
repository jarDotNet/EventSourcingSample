using System.Runtime.ExceptionServices;

namespace EventSourcingSample.ROP;

/// <summary>
/// Provides extension methods to chain two methods, the output of the first is the input of the second.
/// </summary>
/// 
/// <remarks>About Capture and throw, see post: 
/// <see href="https://berserkerdotnet.github.io/blog/rethrow-exception-correctly-in-dotnet">How to rethrow exception correctly in .Net</see>
/// </remarks>
public static class Result_Bind
{
    public static Result<U> Bind<T, U>(this Result<T> r, Func<T, Result<U>> method)
    {
        try
        {
            return r.Success
                ? method(r.Value)
                : Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception ex)
        {
            ExceptionDispatchInfo.Capture(ex).Throw();
            throw;
        }
    }

    public static async Task<Result<U>> Bind<T, U>(this Task<Result<T>> result, Func<T, Task<Result<U>>> method)
    {
        try
        {
            var r = await result;
            return r.Success
                ? await method(r.Value)
                : Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<Result<U>> Bind<T, U>(this Task<Result<T>> result, Func<T, Result<U>> method)
    {
        try
        {
            var r = await result;

            return r.Success
                ? method(r.Value)
                : Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}
