namespace Vector_API.DTOs.Auth.Requests
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;
    }
}
