namespace Vector_API.Entities
{
    public class Usuario
    {
        public int Id { get; set;}
        public string Nome { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }

        //Relacionamento//
        public ICollection<Movimentacao> Movimentacoes { get; set; } 
            = new List<Movimentacao>();
        public ICollection<MetaFinanceira> MetasFinanceiras { get; set; } 
            = new List<MetaFinanceira>();

    }
}
