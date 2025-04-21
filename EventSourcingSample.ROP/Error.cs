namespace EventSourcingSample.ROP;

/// <summary>
/// Represents an error that occurred during the execution of a Result.
/// </summary>
public class Error
{
    public readonly string Message;
    public readonly Guid? ErrorCode;

    private Error(string message, Guid? errorCode)
    {
        Message = message;
        ErrorCode = errorCode;
    }

    public static Error Create(string message, Guid? errorCode = null)
    {
        return new Error(message, errorCode);
    }
}
