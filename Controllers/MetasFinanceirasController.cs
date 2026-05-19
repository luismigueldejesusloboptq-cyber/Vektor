using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vector_API.Data;
using Vector_API.DTOs.MetaFinanceira.Requests;
using Vector_API.DTOs.MetaFinanceira.Responses;
using Vector_API.Entities;

namespace Vector_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MetasFinanceirasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MetasFinanceirasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/metasfinanceiras
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var metas = await _context.MetasFinanceiras
                .Where(m => m.UsuarioId == usuarioId)
                .Select(m => new MetaFinanceiraResponseDto
                {
                    Id = m.Id,
                    Titulo = m.Titulo,      // Mapeado corretamente da Entity (m.Titulo)
                    ValorMeta = m.ValorMeta,
                    ValorAtual = m.ValorAtual,
                    Status = m.Status,
                    Prazo = m.Prazo,
                    DataLimite = m.Prazo ?? DateTime.MinValue // Alinha o Prazo com a DataLimite do DTO
                })
                .ToListAsync();

            return Ok(metas);
        }

        // GET: api/metasfinanceiras/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var meta = await _context.MetasFinanceiras
                .Where(m => m.Id == id && m.UsuarioId == usuarioId)
                .Select(m => new MetaFinanceiraResponseDto
                {
                    Id = m.Id,
                    Titulo = m.Titulo,      // Mapeado corretamente da Entity
                    ValorMeta = m.ValorMeta,
                    ValorAtual = m.ValorAtual,
                    Status = m.Status,
                    Prazo = m.Prazo,
                    DataLimite = m.Prazo ?? DateTime.MinValue
                })
                .FirstOrDefaultAsync();

            if (meta == null)
            {
                return NotFound(new { message = "Meta não encontrada" });
            }

            return Ok(meta);
        }

        // POST: api/metasfinanceiras
        [HttpPost]
        public async Task<IActionResult> Create(CreateMetaFinanceiraRequestDto dto)
        {
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var meta = new MetaFinanceira
            {
                Titulo = dto.Nome,          // Salva o 'Nome' do DTO no campo 'Titulo' do Banco
                ValorMeta = dto.ValorMeta,
                ValorAtual = dto.ValorAtual,
                Prazo = dto.DataLimite,     // Salva a 'DataLimite' do DTO no campo 'Prazo' do Banco
                UsuarioId = usuarioId,
                DataCriacao = DateTime.UtcNow
            };

            _context.MetasFinanceiras.Add(meta);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Meta criada com sucesso" });
        }

        // PUT: api/metasfinanceiras/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMetaFinanceiraRequestDto dto)
        {
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var meta = await _context.MetasFinanceiras
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == usuarioId);

            if (meta == null)
            {
                return NotFound(new { message = "Meta não encontrada" });
            }

            meta.Titulo = dto.Nome;         // Atualiza o 'Titulo' na Entity usando o 'Nome' do DTO
            meta.ValorMeta = dto.ValorMeta;
            meta.ValorAtual = dto.ValorAtual;
            meta.Prazo = dto.DataLimite;    // Atualiza o 'Prazo' na Entity usando a 'DataLimite' do DTO

            await _context.SaveChangesAsync();

            return Ok(new { message = "Meta atualizada com sucesso" });
        }

        // DELETE: api/metasfinanceiras/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst("nameid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var meta = await _context.MetasFinanceiras
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == usuarioId);

            if (meta == null)
            {
                return NotFound(new { message = "Meta não encontrada" });
            }

            _context.MetasFinanceiras.Remove(meta);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Meta removida com sucesso" });
        }
    }
}