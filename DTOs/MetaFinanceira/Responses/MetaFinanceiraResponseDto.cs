namespace Vector_API.DTOs.MetaFinanceira.Responses
{
    public class MetaFinanceiraResponseDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public decimal ValorMeta { get; set; }

        public decimal ValorAtual { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime? Prazo { get; set; }
        public DateTime DataLimite { get; set; }
    }
}
