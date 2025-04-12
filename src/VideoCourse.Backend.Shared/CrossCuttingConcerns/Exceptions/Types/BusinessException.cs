namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;

/// <summary>
/// Represents an exception that is thrown when an business error occurs.
/// </summary>
/// <remarks>
/// This exception is used to indicate that an business error has occurred during the execution of an API request.
/// </remarks>
public class BusinessException : System.Exception
{
    public BusinessException() { }

    public BusinessException(string? message)
        : base(message) { }

    public BusinessException(string? message, System.Exception? innerException)
        : base(message, innerException) { }
}