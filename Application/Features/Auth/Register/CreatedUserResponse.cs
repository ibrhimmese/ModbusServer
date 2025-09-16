namespace Application.Features.Auth.Register;

public class CreatedUserResponse
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }

    public CreatedUserResponse()
    {
        Message = string.Empty;
    }
}
