namespace Application.ExceptionTypes;

public class AuthenticationException:Exception
{
    public AuthenticationException() : base("Bu İşlem İçin Yetkiniz Bulunmuyor")
    { }

    public AuthenticationException(string? message)
        : base(message) { }

    public AuthenticationException(string? message, System.Exception? innerException)
        : base(message, innerException) { }
}
