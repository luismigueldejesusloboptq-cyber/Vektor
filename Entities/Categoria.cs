namespace Vector_API.Entities
{
    public class Categoria
    {
        public int id {  get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;

        //Relacinamento//
        public Usuario Usuario { get; set; } = null;
        public int UsuarioId { get; set; }
        public ICollection<Movimentacao> Movimentacoes { get; set; } 
            = new List<Movimentacao>();
    }
}
