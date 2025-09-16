namespace Application.JwtTokenHandlerInterface.DTOs
{
    public class CreateUserResponseDTO
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }

        public CreateUserResponseDTO()
        {
            Message = string.Empty;
        }
    }
}
