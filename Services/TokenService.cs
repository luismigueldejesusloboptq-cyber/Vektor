using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vector_API.Entities;
using Vector_API.Services.Interfaces;

namespace Vector_API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Usuario usuario)
        {
            var key = Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            );

            var listaClaims = new List<Claim>
            {
                new Claim("nameid", usuario.Id.ToString()),
                new Claim("unique_name", usuario.Nome),
                new Claim("email", usuario.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(listaClaims),

                Expires = DateTime.UtcNow.AddHours(8),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),

                Issuer = _configuration["Jwt:Issuer"],

                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(
                tokenDescriptor
            );

            return tokenHandler.WriteToken(token);
        }
    }
}