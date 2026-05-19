using Vector_API.Entities;

namespace Vector_API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);
    }
}
