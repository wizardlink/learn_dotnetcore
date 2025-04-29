namespace Exceptions;

public class InvalidPersonIDException : ArgumentException
{
    public InvalidPersonIDException() { }

    public InvalidPersonIDException(string? message)
        : base(message) { }

    public InvalidPersonIDException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public InvalidPersonIDException(string? message, string? paramName)
        : base(message, paramName) { }

    public InvalidPersonIDException(string? message, string? paramName, Exception? innerException)
        : base(message, paramName, innerException) { }
}
