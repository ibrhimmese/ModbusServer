namespace Application.ExceptionTypes;

public class AuthorizationException : System.Exception
{
    public AuthorizationException() : base("Oturum açmanız gerekmektedir")
        { }

    public AuthorizationException(string? message)
        : base(message) { }

    public AuthorizationException(string? message, System.Exception? innerException)
        : base(message, innerException) { }
}
