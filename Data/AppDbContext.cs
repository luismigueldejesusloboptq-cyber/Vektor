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

            // 1. Configurações de Usuário
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // 2. Configurações de Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(c => c.id).HasColumnName("Id"); // Mapeia o id minúsculo para o Id maiúsculo do banco

                // Mapeia explicitamente o relacionamento 1:N com Usuário
                entity.HasOne(c => c.Usuario)
                      .WithMany()
                      .HasForeignKey(c => c.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 3. Configurações de Movimentacao
            modelBuilder.Entity<Movimentacao>(entity =>
            {
                entity.Property(m => m.Valor).HasPrecision(10, 2);

                // Configura o relacionamento com Categoria
                entity.HasOne(m => m.Categoria)
                      .WithMany(c => c.Movimentacoes)
                      .HasForeignKey(m => m.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configura o relacionamento com Usuário
                entity.HasOne(m => m.Usuario)
                      .WithMany(u => u.Movimentacoes)
                      .HasForeignKey(m => m.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 4. Configurações de MetaFinanceira
            modelBuilder.Entity<MetaFinanceira>(entity =>
            {
                entity.Property(m => m.ValorMeta).HasPrecision(10, 2);
                entity.Property(m => m.ValorAtual).HasPrecision(10, 2);
            });
        }
    }
}