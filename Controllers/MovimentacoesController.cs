using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vector_API.Data;
using Vector_API.DTOs.Movimentacao.Requests;
using Vector_API.DTOs.Movimentacao.Responses;
using Vector_API.Entities;

namespace Vector_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MovimentacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovimentacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/movimentacoes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var movimentacoes = await _context.Movimentacoes
                .Include(m => m.Categoria)
                .Where(m => m.UsuarioId == usuarioId)
                .Select(m => new MovimentacaoResponseDto
                {
                    Id = m.Id,
                    Descricao = m.Descricao,
                    Valor = m.Valor,
                    Data = m.Data,
                    Tipo = m.Tipo,
                    Categoria = m.Categoria.Nome
                })
                .ToListAsync();

            return Ok(movimentacoes);
        }

        // GET: api/movimentacoes/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var movimentacao = await _context.Movimentacoes
                .Include(m => m.Categoria)
                .Where(m =>
                    m.Id == id &&
                    m.UsuarioId == usuarioId)
                .Select(m => new MovimentacaoResponseDto
                {
                    Id = m.Id,
                    Descricao = m.Descricao,
                    Valor = m.Valor,
                    Data = m.Data,
                    Tipo = m.Tipo,
                    Categoria = m.Categoria.Nome
                })
                .FirstOrDefaultAsync();

            if (movimentacao == null)
                return NotFound();

            return Ok(movimentacao);
        }

        // POST: api/movimentacoes
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateMovimentacaoRequestDto dto)
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var movimentacao = new Movimentacao
            {
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Data = dto.Data,
                Tipo = dto.Tipo,
                CategoriaId = dto.CategoriaId,
                UsuarioId = usuarioId
            };

            _context.Movimentacoes.Add(movimentacao);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Movimentação criada com sucesso"
            });
        }

        // PUT: api/movimentacoes/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateMovimentacaoRequestDto dto)
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var movimentacao = await _context.Movimentacoes
                .FirstOrDefaultAsync(m =>
                    m.Id == id &&
                    m.UsuarioId == usuarioId);

            if (movimentacao == null)
                return NotFound(new
                {
                    message = "Movimentação não encontrada"
                });

            movimentacao.Descricao = dto.Descricao;
            movimentacao.Valor = dto.Valor;
            movimentacao.Data = dto.Data;
            movimentacao.Tipo = dto.Tipo;
            movimentacao.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Movimentação atualizada com sucesso"
            });
        }

        // DELETE: api/movimentacoes/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var movimentacao = await _context.Movimentacoes
                .FirstOrDefaultAsync(m =>
                    m.Id == id &&
                    m.UsuarioId == usuarioId);

            if (movimentacao == null)
                return NotFound(new
                {
                    message = "Movimentação não encontrada"
                });

            _context.Movimentacoes.Remove(movimentacao);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Movimentação removida com sucesso"
            });
        }
    }
}