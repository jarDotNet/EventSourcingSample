using System.Collections.Immutable;
using System.Net;

namespace EventSourcingSample.ROP;

/// <summary>
/// Represents the result of an operation, containing either a value of type T or a collection of errors.
/// </summary>
/// <typeparam name="T">The type of the value contained in the result.</typeparam>
public struct Result<T>
{
    public readonly T Value;

    public readonly ImmutableArray<Error> Errors;

    public readonly HttpStatusCode HttpStatusCode;

    public readonly bool Success => Errors.Length == 0;

    public Result(T value, HttpStatusCode statusCode)
    {
        Value = value;
        Errors = [];
        HttpStatusCode = statusCode;
    }

    public Result(ImmutableArray<Error> errors, HttpStatusCode statusCode)
    {
        if (!errors.Any())
        {
            throw new InvalidOperationException("You should specify at least one error");
        }

        HttpStatusCode = statusCode;
        Value = default!;
        Errors = errors;
    }

    public static implicit operator Result<T>(T value) => new(value, HttpStatusCode.OK);

    public static implicit operator Result<T>(ImmutableArray<Error> errors) => new(errors, HttpStatusCode.BadRequest);
}
