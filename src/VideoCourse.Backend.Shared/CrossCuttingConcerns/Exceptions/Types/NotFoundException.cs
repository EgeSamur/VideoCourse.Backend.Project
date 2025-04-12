namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;

/// <summary>
/// Represents an exception that is thrown when an not found error occurs.
/// </summary>
/// <remarks>
/// This exception is used to indicate that an not found error has occurred during the execution of an API request.
/// </remarks>
public class NotFoundException : System.Exception
{
    public NotFoundException() { }

    public NotFoundException(string? message)
        : base(message) { }

    public NotFoundException(string? message, System.Exception? innerException)
        : base(message, innerException) { }
}