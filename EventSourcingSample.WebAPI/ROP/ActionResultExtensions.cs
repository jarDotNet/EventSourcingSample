using EventSourcingSample.ROP;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventSourcingSample.WebAPI.ROP;

internal static class ActionResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.ToDto().ToHttpStatusCode(result.HttpStatusCode);
    }

    public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> result)
    {
        Result<T> r = await result;

        return r.ToActionResult();
    }

    private static ResultWithStatusCode<T> ToHttpStatusCode<T>(this T resultDto, HttpStatusCode statusCode)
    {
        return new ResultWithStatusCode<T>(resultDto, statusCode);
    }

    private class ResultWithStatusCode<T> : ObjectResult
    {
        public ResultWithStatusCode(T content, HttpStatusCode httpStatusCode)
            : base(content)
        {
            StatusCode = (int)httpStatusCode;
        }
    }
}
