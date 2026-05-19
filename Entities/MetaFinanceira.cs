namespace Vector_API.Entities
{
    public class MetaFinanceira
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public decimal ValorMeta { get; set; }

        public decimal ValorAtual { get; set; }

        public DateTime? Prazo { get; set; }

        public string Status { get; set; } = "EM_ANDAMENTO";

        public DateTime DataCriacao { get; set; }

        // Relacionamento//
        public Usuario Usuario { get; set; } = null!;
    }
}
