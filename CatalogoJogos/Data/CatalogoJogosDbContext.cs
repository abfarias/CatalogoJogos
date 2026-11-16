using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Data
{
    public class CatalogoJogosDbContext : DbContext, IUnidadeDeTrabalho
    {
        public CatalogoJogosDbContext(DbContextOptions<CatalogoJogosDbContext> options) : base(options) { }

        public DbSet<Administrador> Administradores => Set<Administrador>();
        public DbSet<Jogador> Jogadores => Set<Jogador>();
        public DbSet<Jogo> Jogos => Set<Jogo>();
        public DbSet<Compra> Compras => Set<Compra>();
        public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();

        public Task<int> SalvarAlteracoesAsync()
        {
            return SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>(e =>
            {
                e.ToTable("Administrador");
                e.HasKey(a => a.IdAdmin);
                e.Property(a => a.Nome).HasMaxLength(100).IsRequired();
                e.Property(a => a.Email).HasMaxLength(256).IsRequired();
                e.Property(a => a.SenhaHash).HasMaxLength(256).IsRequired();
                e.HasIndex(a => a.Email).IsUnique();
            });

            modelBuilder.Entity<Jogador>(e =>
            {
                e.ToTable("Jogador", t =>
                    t.HasCheckConstraint("CK_Jogador_SaldoCarteira", "[SaldoCarteira] >= 0"));
                e.HasKey(j => j.IdJogador);
                e.Property(j => j.Apelido).HasMaxLength(50).IsRequired();
                e.Property(j => j.Email).HasMaxLength(256).IsRequired();
                e.Property(j => j.SenhaHash).HasMaxLength(256).IsRequired();
                e.Property(j => j.FotoPerfil).HasMaxLength(500);
                e.Property(j => j.SaldoCarteira).HasPrecision(10, 2).HasDefaultValue(0m);
                e.HasIndex(j => j.Email).IsUnique();
                e.HasIndex(j => j.Apelido).IsUnique();
            });

            modelBuilder.Entity<Jogo>(e =>
            {
                e.ToTable("Jogo", t =>
                    t.HasCheckConstraint("CK_Jogo_Preco", "[Preco] >= 0 AND [Preco] <= 9999.99"));
                e.HasKey(j => j.IdJogo);
                e.Property(j => j.Titulo).HasMaxLength(100).IsRequired();
                e.Property(j => j.Descricao).HasMaxLength(1000).IsRequired();
                e.Property(j => j.Categoria).HasMaxLength(50).IsRequired();
                e.Property(j => j.Preco).HasPrecision(10, 2);
                e.Property(j => j.CaminhoFotoCapa).HasMaxLength(500);
                // Padrão 1 no banco; o EF sempre envia o valor da entidade (inclusive false).
                e.Property(j => j.Ativo).HasDefaultValue(true).ValueGeneratedNever();
                e.HasIndex(j => j.Categoria);

                e.HasOne(j => j.AdminCadastro)
                    .WithMany(a => a.JogosCadastrados)
                    .HasForeignKey(j => j.IdAdminCadastro)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Compra>(e =>
            {
                e.ToTable("Compra", t =>
                    t.HasCheckConstraint("CK_Compra_ValorDebitado", "[ValorDebitado] >= 0"));
                e.HasKey(c => c.IdCompra);
                e.Property(c => c.ValorDebitado).HasPrecision(10, 2);
                e.HasIndex(c => new { c.IdJogador, c.IdJogo }).IsUnique();

                e.HasOne(c => c.Jogador)
                    .WithMany(j => j.Compras)
                    .HasForeignKey(c => c.IdJogador)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(c => c.Jogo)
                    .WithMany(j => j.Compras)
                    .HasForeignKey(c => c.IdJogo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Avaliacao>(e =>
            {
                e.ToTable("Avaliacao", t =>
                    t.HasCheckConstraint("CK_Avaliacao_Nota", "[Nota] BETWEEN 1 AND 5"));
                e.HasKey(a => a.IdAvaliacao);
                e.Property(a => a.Comentario).HasMaxLength(500);
                e.HasIndex(a => new { a.IdJogador, a.IdJogo }).IsUnique();

                e.HasOne(a => a.Jogador)
                    .WithMany(j => j.Avaliacoes)
                    .HasForeignKey(a => a.IdJogador)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(a => a.Jogo)
                    .WithMany(j => j.Avaliacoes)
                    .HasForeignKey(a => a.IdJogo)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
