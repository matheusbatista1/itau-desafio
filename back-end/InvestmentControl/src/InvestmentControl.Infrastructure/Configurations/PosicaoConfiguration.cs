using InvestmentControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestmentControl.Infrastructure.Configurations;

public class PosicaoConfiguration : IEntityTypeConfiguration<Posicao>
{
    public void Configure(EntityTypeBuilder<Posicao> builder)
    {
        builder.ToTable("posicoes");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.Property(p => p.AtivoId)
            .HasColumnName("ativo_id")
            .IsRequired();

        builder.Property(p => p.Quantidade)
            .HasColumnName("quantidade")
            .IsRequired();

        builder.Property(p => p.PrecoMedio)
            .HasColumnName("preco_medio")
            .HasColumnType("decimal(15,4)")
            .IsRequired();

        builder.Property(p => p.PL)
            .HasColumnName("pl")
            .HasColumnType("decimal(15,2)")
            .IsRequired();

        builder.HasOne(p => p.Usuario)
            .WithMany(a => a.Posicoes)
            .HasForeignKey(p => p.UsuarioId);

        builder.HasOne(p => p.Ativo)
            .WithMany(a => a.Posicoes)
            .HasForeignKey(p => p.AtivoId);

        builder.HasIndex(p => new { p.UsuarioId, p.AtivoId })
            .IsUnique()
            .HasDatabaseName("idx_posicoes_usuario_ativo");
    }
}