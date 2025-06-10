using InvestmentControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestmentControl.Infrastructure.Configurations;

public class CotacaoConfiguration : IEntityTypeConfiguration<Cotacao>
{
    public void Configure(EntityTypeBuilder<Cotacao> builder)
    {
        builder.ToTable("cotacoes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.AtivoId)
            .HasColumnName("ativo_id")
            .IsRequired();

        builder.Property(c => c.PrecoUnitario)
            .HasColumnName("preco_unitario")
            .HasColumnType("decimal(15,4)")
            .IsRequired();

        builder.Property(c => c.DataHora)
            .HasColumnName("data_hora")
            .IsRequired();

        builder.HasOne(c => c.Ativo)
            .WithMany(a => a.Cotacoes)
            .HasForeignKey(c => c.AtivoId);

        builder.HasIndex(c => new { c.AtivoId, c.DataHora })
            .HasDatabaseName("idx_cotacoes_ativo_data");
    }
}