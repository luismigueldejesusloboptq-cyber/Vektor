namespace Vector_API.DTOs.Auth.Requests
{
    public class RegisterRequestDto
    {
        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;
    }
}
