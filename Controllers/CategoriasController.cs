using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vector_API.Data;
using Vector_API.DTOs.Categoria.Requests;
using Vector_API.DTOs.Categoria.Responses;
using Vector_API.Entities;

namespace Vector_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Rota gerada: api/categorias
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Lê de forma segura tanto o padrão JWT puro quanto o do .NET
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var categories = await _context.Categorias
                .Where(c => c.UsuarioId == usuarioId)
                .Select(c => new CategoriaResponseDto
                {
                    Id = c.id,
                    Nome = c.Nome,
                    Tipo = c.Tipo
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/categorias/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var categoria = await _context.Categorias
                .Where(c => c.id == id && c.UsuarioId == usuarioId)
                .Select(c => new CategoriaResponseDto
                {
                    Id = c.id,
                    Nome = c.Nome,
                    Tipo = c.Tipo
                })
                .FirstOrDefaultAsync();

            if (categoria == null)
                return NotFound(new { message = $"Categoria com ID {id} não encontrada para este usuário." });

            return Ok(categoria);
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<IActionResult> Create(CategoriaRequestDto dto)
        {
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var categoria = new Categoria
            {
                Nome = dto.Nome,
                Tipo = dto.Tipo,
                UsuarioId = usuarioId
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return Ok(new CategoriaResponseDto
            {
                Id = categoria.id,
                Nome = categoria.Nome,
                Tipo = categoria.Tipo
            });
        }

        // PUT: api/categorias/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoriaRequestDto dto)
        {
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.id == id && c.UsuarioId == usuarioId);

            if (categoria == null)
                return NotFound(new { message = "Categoria não encontrada ou não pertence a este usuário." });

            categoria.Nome = dto.Nome;
            categoria.Tipo = dto.Tipo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/categorias/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.id == id && c.UsuarioId == usuarioId);

            if (categoria == null)
                return NotFound(new { message = "Categoria não encontrada ou não pertence a este usuário." });

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}