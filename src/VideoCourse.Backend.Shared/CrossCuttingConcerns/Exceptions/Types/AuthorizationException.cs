namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;


/// <summary>
/// Represents an exception that is thrown when an authorization error occurs.
/// </summary>
/// <remarks>
/// This exception is used to indicate that an authorization error has occurred during the execution of an API request.
/// </remarks>
public class AuthorizationException : System.Exception
{
    public AuthorizationException() { }

    public AuthorizationException(string? message)
        : base(message) { }

    public AuthorizationException(string? message, System.Exception? innerException)
        : base(message, innerException) { }
}