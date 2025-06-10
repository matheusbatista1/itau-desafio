using InvestmentControl.Domain.Entities;
using InvestmentControl.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InvestmentControl.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Operacao> Operacoes { get; set; }
    public DbSet<Ativo> Ativos { get; set; }
    public DbSet<Cotacao> Cotacoes { get; set; }
    public DbSet<Posicao> Posicoes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OperacoesConfiguration());
        modelBuilder.ApplyConfiguration(new AtivoConfiguration());
        modelBuilder.ApplyConfiguration(new CotacaoConfiguration());
        modelBuilder.ApplyConfiguration(new PosicaoConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}