using System.Collections.Immutable;
using System.Net;

namespace EventSourcingSample.ROP;

/// <summary>
/// Provides extension methods to create Result objects.
/// </summary>
public static class Result
{
    public static Result<T> Success<T>(this T value) => new(value, HttpStatusCode.OK);
    public static Result<T> Success<T>(this T value, HttpStatusCode httpStatusCode) => new(value, httpStatusCode);
    public static Result<Unit> Success() => new(Unit.Value, HttpStatusCode.OK);

    public static Task<Result<T>> Async<T>(this Result<T> r) => Task.FromResult(r);

    public static Result<T> Failure<T>(ImmutableArray<Error> errors) => new(errors, HttpStatusCode.InternalServerError);
    public static Result<T> Failure<T>(ImmutableArray<Error> errors, HttpStatusCode httpStatusCode) => new(errors, httpStatusCode);
    public static Result<T> Failure<T>(Error error) => new([error], HttpStatusCode.InternalServerError);
    public static Result<T> Failure<T>(string error) => new([Error.Create(error)], HttpStatusCode.InternalServerError);
    public static Result<Unit> Failure(ImmutableArray<Error> errors) => new(errors, HttpStatusCode.InternalServerError);
    public static Result<Unit> Failure(ImmutableArray<Error> errors, HttpStatusCode httpStatusCode) => new(errors, httpStatusCode);
    public static Result<Unit> Failure(Error error) => new([error], HttpStatusCode.InternalServerError);
    public static Result<Unit> Failure(string error) => new([Error.Create(error)], HttpStatusCode.InternalServerError);

    public static Result<T> BadRequest<T>(ImmutableArray<Error> errors) => new(errors, HttpStatusCode.BadRequest);
    public static Result<T> BadRequest<T>(Error error) => new([error], HttpStatusCode.BadRequest);
    public static Result<T> BadRequest<T>(string error) => new([Error.Create(error)], HttpStatusCode.BadRequest);
    public static Result<Unit> BadRequest(ImmutableArray<Error> errors) => new(errors, HttpStatusCode.BadRequest);
    public static Result<Unit> BadRequest(Error error) => new([error], HttpStatusCode.BadRequest);
    public static Result<Unit> BadRequest(string error) => new([Error.Create(error)], HttpStatusCode.BadRequest);

    public static Result<T> NotFound<T>(string error) => new([Error.Create(error)], HttpStatusCode.NotFound);
    public static Result<Unit> NotFound(string error) => new([Error.Create(error)], HttpStatusCode.NotFound);
}

