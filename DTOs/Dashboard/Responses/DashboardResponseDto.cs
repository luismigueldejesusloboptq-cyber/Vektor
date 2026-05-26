namespace Vector_API.DTOs.Dashboard.Responses
{
    public class DashboardResponseDto
    {
        public decimal SaldoTotal { get; set; }

        public decimal ReceitasMes { get; set; }

        public decimal DespesasMes { get; set; }

        public decimal MediaMensal { get; set; }

        public List<UltimaMovimentacaoDto>
            UltimasMovimentacoes
        { get; set; }
            = new();

        public List<GraficoFinanceiroDto>
            GraficoFinanceiro
        { get; set; }
            = new();
    }

    public class UltimaMovimentacaoDto
    {
        public string Descricao { get; set; }
            = string.Empty;

        public decimal Valor { get; set; }

        public string Tipo { get; set; }
            = string.Empty;

        public DateTime Data { get; set; }
    }

    public class GraficoFinanceiroDto
    {
        public string Mes { get; set; }
            = string.Empty;

        public decimal Receitas { get; set; }

        public decimal Despesas { get; set; }

        public List<GastosCategoriaDto> GastosPorCategoria { get; set; }

        public List<HistoricoMensalDto> HistoricoMensal { get; set; }
    }
}