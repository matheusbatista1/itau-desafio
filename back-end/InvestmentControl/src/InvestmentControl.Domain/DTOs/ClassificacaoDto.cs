namespace InvestmentControl.Domain.DTOs;

public class ClassificacaoDto
{
    public int UsuarioId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}