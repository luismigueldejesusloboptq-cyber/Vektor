
using Microsoft.EntityFrameworkCore;
using Vector_API;
using Vector_API.Entities;
namespace Vector_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Movimentacao> Movimentacoes { get; set; }

        public DbSet<MetaFinanceira> MetasFinanceiras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Movimentacao
            modelBuilder.Entity<Movimentacao>()
                .Property(m => m.Valor)
                .HasPrecision(10, 2);

            // MetaFinanceira
            modelBuilder.Entity<MetaFinanceira>()
                .Property(m => m.ValorMeta)
                .HasPrecision(10, 2);

            modelBuilder.Entity<MetaFinanceira>()
                .Property(m => m.ValorAtual)
                .HasPrecision(10, 2);
        }
    }
}
