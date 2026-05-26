namespace Vector_API.DTOs.Dashboard.Responses
{
    public class HistoricoMensalDto
    {
        public string Mes { get; set; } = string.Empty;

        public decimal Receitas { get; set; }

        public decimal Despesas { get; set; }
    }
}