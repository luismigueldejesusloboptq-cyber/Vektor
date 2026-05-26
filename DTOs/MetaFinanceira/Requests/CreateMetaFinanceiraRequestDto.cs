namespace Vector_API.DTOs.MetaFinanceira.Requests
{
    public class CreateMetaFinanceiraRequestDto
    {
        public string Nome { get; set; } = string.Empty;

        public decimal ValorMeta { get; set; }

        public decimal ValorAtual { get; set; }

        public DateTime DataLimite { get; set; }
    }
}
