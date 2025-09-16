using Application.JwtTokenHandlerInterface;

namespace Application.Features.Auth.Login;

public class LoginUserCommandResponse
{

}

public class LoginUserCommandSuccessResponse : LoginUserCommandResponse
{
    public Token? Token { get; set; }

}
public class LoginUserCommandErrorResponse : LoginUserCommandResponse
{
    public string Message { get; set; }
    public LoginUserCommandErrorResponse(string message)
    {
        Message = message;
    }
}