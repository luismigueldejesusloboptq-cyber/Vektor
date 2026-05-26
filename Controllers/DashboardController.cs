using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vector_API.Data;
using Vector_API.DTOs.Dashboard.Responses;

namespace Vector_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var usuarioIdClaim =
                User.FindFirst("nameId")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (usuarioIdClaim == null)
            {
                return Unauthorized(new
                {
                    message = "Usuário não autenticado"
                });
            }

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return BadRequest(new
                {
                    message = "ID do usuário inválido"
                });
            }

            var movimentacoes = await _context.Movimentacoes
                .Include(m => m.Categoria)
                .Where(m => m.UsuarioId == usuarioId)
                .ToListAsync();

            // =========================
            // RESUMO FINANCEIRO
            // =========================

            var receitas = movimentacoes
                .Where(m => m.Tipo == "RECEITA")
                .Sum(m => m.Valor);

            var despesas = movimentacoes
                .Where(m => m.Tipo == "DESPESA")
                .Sum(m => m.Valor);

            var saldoTotal = receitas - despesas;

            var receitasMes = movimentacoes
                .Where(m =>
                    m.Tipo == "RECEITA" &&
                    m.Data.Month == DateTime.Now.Month &&
                    m.Data.Year == DateTime.Now.Year
                )
                .Sum(m => m.Valor);

            var despesasMes = movimentacoes
                .Where(m =>
                    m.Tipo == "DESPESA" &&
                    m.Data.Month == DateTime.Now.Month &&
                    m.Data.Year == DateTime.Now.Year
                )
                .Sum(m => m.Valor);

            var mediaMensal = movimentacoes.Any()
                ? movimentacoes.Average(m => m.Valor)
                : 0;

            // =========================
            // ÚLTIMAS MOVIMENTAÇÕES
            // =========================

            var ultimasMovimentacoes = movimentacoes
                .OrderByDescending(m => m.Data)
                .Take(5)
                .Select(m => new UltimaMovimentacaoDto
                {
                    Descricao = m.Descricao,
                    Valor = m.Valor,
                    Tipo = m.Tipo,
                    Data = m.Data
                })
                .ToList();

            // =========================
            // GRÁFICO FINANCEIRO
            // =========================

            var graficoFinanceiro = movimentacoes
                .GroupBy(m => m.Data.Month)
                .Select(g => new GraficoFinanceiroDto
                {
                    Mes = System.Globalization
                        .CultureInfo
                        .CurrentCulture
                        .DateTimeFormat
                        .GetAbbreviatedMonthName(g.Key),

                    Receitas = g
                        .Where(x => x.Tipo == "RECEITA")
                        .Sum(x => x.Valor),

                    Despesas = g
                        .Where(x => x.Tipo == "DESPESA")
                        .Sum(x => x.Valor)
                })
                .OrderBy(g => g.Mes)
                .ToList();

            // =========================
            // DADOS EXTRAS TEMPORÁRIOS
            // =========================

            var graficoCategorias = movimentacoes
                .Where(m => m.Tipo == "DESPESA")
                .GroupBy(m => m.Categoria.Nome)
                .Select(g => new
                {
                    categoria = g.Key,
                    valor = g.Sum(x => x.Valor)
                })
                .ToList();

            var graficoUltimosMeses = movimentacoes
                .GroupBy(m => new
                {
                    m.Data.Year,
                    m.Data.Month
                })
                .Select(g => new
                {
                    mes = $"{g.Key.Month}/{g.Key.Year}",

                    receitas = g
                        .Where(x => x.Tipo == "RECEITA")
                        .Sum(x => x.Valor),

                    despesas = g
                        .Where(x => x.Tipo == "DESPESA")
                        .Sum(x => x.Valor)
                })
                .OrderBy(x => x.mes)
                .Take(6)
                .ToList();

            var insights = new List<string>();

            if (despesas > receitas)
            {
                insights.Add(
                    "Suas despesas estão maiores que suas receitas."
                );
            }

            if (receitas > despesas)
            {
                insights.Add(
                    "Seu saldo financeiro está positivo."
                );
            }

            if (despesasMes > receitasMes * 0.8m)
            {
                insights.Add(
                    "Você está gastando mais de 80% da renda mensal."
                );
            }

            // =========================
            // RESPONSE
            // =========================

            return Ok(new
            {
                saldoTotal = saldoTotal,

                receitasMes = receitasMes,

                despesasMes = despesasMes,

                mediaMensal = mediaMensal,

                ultimasMovimentacoes = ultimasMovimentacoes,

                graficoFinanceiro = graficoFinanceiro,

                graficoCategorias = graficoCategorias,

                graficoUltimosMeses = graficoUltimosMeses,

                insights = insights
            });
        }
    }
}