using Vector_API.DTOs.Auth.Requests;
using Vector_API.DTOs.Auth.Responses;

namespace Vector_API.Services.Interfaces
{
    
    public interface IAuthService
    {
     
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);

        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
    }
}