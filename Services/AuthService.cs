using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Vector_API.Data;
using Vector_API.DTOs.Auth.Requests;
using Vector_API.DTOs.Auth.Responses;
using Vector_API.Entities;
using Vector_API.Services.Interfaces;
using Vector_API.Data;
using Vector_API.DTOs.Auth.Requests;
using Vector_API.DTOs.Auth.Responses;
using Vector_API.Entities;
using Vector_API.Services.Interfaces;

namespace Vektor.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        private readonly ITokenService _tokenService;

        public AuthService(
            AppDbContext context,
            ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExiste)
            {
                throw new Exception("Email já cadastrado.");
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                DataCriacao = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            var token = _tokenService.GenerateToken(usuario);

            return new AuthResponseDto
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
            {
                throw new Exception("Email ou senha inválidos.");
            }

            var senhaValida = BCrypt.Net.BCrypt.Verify(
                dto.Senha,
                usuario.SenhaHash
            );

            if (!senhaValida)
            {
                throw new Exception("Email ou senha inválidos.");
            }

            var token = _tokenService.GenerateToken(usuario);

            return new AuthResponseDto
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }
    }
}