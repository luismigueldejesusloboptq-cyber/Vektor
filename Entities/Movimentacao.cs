using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vector_API.Entities
{
    public class Movimentacao
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int CategoriaId { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public string Tipo { get; set; } // 'RECEITA' ou 'DESPESA'

        public DateTime Data { get; set; }

        public DateTime DataCriacao { get; set; }

        // 🔥 DESCOMENTADO: Resolve o erro do AppDbContext relacionado ao Usuário
        public Usuario Usuario { get; set; }

        public Categoria Categoria { get; set; }
    }
}