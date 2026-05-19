namespace Vector_API.Entities
{
    public class Movimentacao
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }

        // VEJA SE ESSA LINHA ESTÁ EXATAMENTE ASSIM:
        public DateTime Data { get; set; }

        public string Tipo { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
        public int UsuarioId { get; set; }
    }
}