using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vector_API.Data;
using Vector_API.DTOs.Movimentacao.Requests;
using Vector_API.DTOs.Movimentacao.Responses;
using Vector_API.Entities;

namespace Vector_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // ⚠️ COMENTADO TEMPORARIAMENTE: Para o Angular conseguir testar a API sem precisar de Token de Login
    // [Authorize] 
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
            // Fixamos o usuário 1 para testes. Quando o JWT for reativado, voltamos o código original.
            var usuarioId = 1;

            var movimentacoes = await _context.Movimentacoes
                .Include(m => m.Categoria)
                .Where(m => m.UsuarioId == usuarioId)
                .Select(m => new MovimentacaoResponseDto
                {
                    Id = m.Id,
                    Titulo = m.Titulo,
                    Descricao = m.Descricao,
                    Valor = m.Valor,
                    Data = m.Data, // 🔥 Corrigido para Data
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
            var usuarioId = 1;

            var movimentacao = await _context.Movimentacoes
                .Include(m => m.Categoria)
                .Where(m => m.Id == id && m.UsuarioId == usuarioId)
                .Select(m => new MovimentacaoResponseDto
                {
                    Id = m.Id,
                    Titulo = m.Titulo,
                    Descricao = m.Descricao,
                    Valor = m.Valor,
                    Data = m.Data, // 🔥 Corrigido para Data
                    Tipo = m.Tipo,
                    Categoria = m.Categoria.Nome
                })
                .FirstOrDefaultAsync();

            if (movimentacao == null)
            {
                return NotFound(new { message = "Movimentação não encontrada" });
            }

            return Ok(movimentacao);
        }

        // POST: api/movimentacoes
        [HttpPost]
        public async Task<IActionResult> Create(CreateMovimentacaoRequestDto dto)
        {
            var movimentacao = new Movimentacao
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Data = dto.Data, // 🔥 Corrigido para Data
                Tipo = dto.Tipo,

                // Garantimos que se o Angular não enviar a categoria corretamente, assume a Categoria 1
                CategoriaId = dto.CategoriaId == 0 ? 1 : dto.CategoriaId,

                // Forçamos o Usuário 1 diretamente para não estourar erro de Foreign Key
                UsuarioId = 1
            };

            _context.Movimentacoes.Add(movimentacao);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Movimentação criada com sucesso" });
        }

        // PUT: api/movimentacoes/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMovimentacaoRequestDto dto)
        {
            var usuarioId = 1;

            var movimentacao = await _context.Movimentacoes
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == usuarioId);

            if (movimentacao == null)
            {
                return NotFound(new { message = "Movimentação não encontrada" });
            }

            movimentacao.Titulo = dto.Titulo;
            movimentacao.Descricao = dto.Descricao ?? "";
            movimentacao.Valor = dto.Valor;
            movimentacao.Data = dto.Data; // 🔥 Corrigido para Data
            movimentacao.Tipo = dto.Tipo;
            movimentacao.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Movimentação atualizada com sucesso" });
        }

        // DELETE: api/movimentacoes/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = 1;

            var movimentacao = await _context.Movimentacoes
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == usuarioId);

            if (movimentacao == null)
            {
                return NotFound(new { message = "Movimentação não encontrada" });
            }

            _context.Movimentacoes.Remove(movimentacao);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Movimentação removida com sucesso" });
        }
    }
}