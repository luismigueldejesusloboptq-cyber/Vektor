using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vector_API.Data;
using Vector_API.DTOs.Usuario.Requests;
using Vector_API.DTOs.Usuario.Responses;

namespace Vector_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios/me
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var usuarioIdClaim =
    User.FindFirst("nameid")?.Value;

            if (usuarioIdClaim == null)
            {
                return Unauthorized(new
                {
                    message = "Usuário não autenticado"
                });
            }

            var usuarioId =
                int.Parse(usuarioIdClaim);

            // CORRIGIDO: Alterado de .Usuario para .Usuarios
            var usuario = await _context.Usuarios
                .Where(u => u.Id == usuarioId)
                .Select(u => new UsuarioResponseDto
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    DataCriacao = u.DataCriacao
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            return Ok(usuario);
        }

        // PUT: api/usuarios/me
        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile(UpdateUsuarioRequestDto dto)
        {
            var usuarioIdClaim =
    User.FindFirst("nameid")?.Value;

            if (usuarioIdClaim == null)
            {
                return Unauthorized(new
                {
                    message = "Usuário não autenticado"
                });
            }

            var usuarioId =
                int.Parse(usuarioIdClaim);

            // CORRIGIDO: Alterado de .Usuario para .Usuarios
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Perfil atualizado com sucesso" });
        }

        // DELETE: api/usuarios/me
        [HttpDelete("me")]
        public async Task<IActionResult> DeleteProfile()
        {
            var usuarioIdClaim =
    User.FindFirst("nameid")?.Value;

            if (usuarioIdClaim == null)
            {
                return Unauthorized(new
                {
                    message = "Usuário não autenticado"
                });
            }

            var usuarioId =
                int.Parse(usuarioIdClaim);

            // CORRIGIDO: Alterado de .Usuario para .Usuarios
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            // CORRIGIDO: Alterado de .Usuario para .Usuarios
            _context.Usuarios.Remove(usuario);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Conta removida com sucesso" });
        }
    }
}