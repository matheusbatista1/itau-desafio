using InvestmentControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestmentControl.Infrastructure.Configurations;

public class AtivoConfiguration : IEntityTypeConfiguration<Ativo>
{
    public void Configure(EntityTypeBuilder<Ativo> builder)
    {
        builder.ToTable("ativos");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Codigo)
            .HasColumnName("codigo")
            .HasMaxLength(10)
            .IsRequired();

        builder.HasIndex(a => a.Codigo)
            .IsUnique();

        builder.Property(a => a.Nome)
            .HasColumnName("nome")
            .HasMaxLength(100)
            .IsRequired();
    }
}