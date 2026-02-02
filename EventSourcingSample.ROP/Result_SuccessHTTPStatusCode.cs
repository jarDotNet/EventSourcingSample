using System.Net;
using System.Runtime.ExceptionServices;

namespace EventSourcingSample.ROP;

/// <summary>
/// Provides extension methods for handling success status codes in result chains.
/// </summary>
public static class Result_SuccessHTTPStatusCode
{
    public static Result<T> UseSuccessHttpStatusCode<T>(this Result<T> r, HttpStatusCode httpStatusCode)
    {
        try
        {
            return r.Success
                ? Result.Success(r.Value, httpStatusCode)
                : Result.Failure<T>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<Result<T>> UseSuccessHttpStatusCode<T>(this Task<Result<T>> result, HttpStatusCode httpStatusCode)
    {
        try
        {
            var r = await result;
            return r.UseSuccessHttpStatusCode(httpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}