namespace Vector_API.DTOs.Movimentacao.Requests
{
    public class CreateMovimentacaoRequestDto
    {
        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public int CategoriaId { get; set; }

        // Alterado de DataMovimentacao para Data
        public DateTime Data { get; set; }

        public int UsuarioId { get; set; }
    }
}