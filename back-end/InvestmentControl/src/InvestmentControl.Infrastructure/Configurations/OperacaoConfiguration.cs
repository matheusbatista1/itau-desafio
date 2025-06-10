using InvestmentControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestmentControl.Infrastructure.Configurations;

public class OperacoesConfiguration : IEntityTypeConfiguration<Operacao>
{
    public void Configure(EntityTypeBuilder<Operacao> builder)
    {
        builder.ToTable("operacoes");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(o => o.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.Property(o => o.AtivoId)
            .HasColumnName("ativo_id")
            .IsRequired();

        builder.Property(o => o.Quantidade)
            .HasColumnName("quantidade")
            .IsRequired();

        builder.Property(o => o.PrecoUnitario)
            .HasColumnName("preco_unitario")
            .HasColumnType("decimal(15,4)")
            .IsRequired();

        builder.Property(o => o.TipoOperacao)
            .HasColumnName("tipo_operacao")
            .HasConversion<string>() // Armazena como texto
            .IsRequired();

        builder.Property(o => o.Corretagem)
            .HasColumnName("corretagem")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(o => o.DataHora)
            .HasColumnName("data_hora")
            .IsRequired();

        builder.HasIndex(o => new { o.UsuarioId, o.AtivoId, o.DataHora })
            .HasDatabaseName("idx_operacoes_usuario_ativo_data");

        builder.HasOne(o => o.Usuario)
            .WithMany(a => a.Operacoes)
            .HasForeignKey(o => o.UsuarioId);

        builder.HasOne(o => o.Ativo)
            .WithMany(a => a.Operacoes)
            .HasForeignKey(o => o.AtivoId);
    }
}