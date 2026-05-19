namespace Vector_API.DTOs.Movimentacao.Requests
{
    public class UpdateMovimentacaoRequestDto
    {
        public int CategoriaId { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public decimal Valor { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public DateTime DataMovimentacao { get; set; }
        public DateTime Data { get; set; }
    }
}
