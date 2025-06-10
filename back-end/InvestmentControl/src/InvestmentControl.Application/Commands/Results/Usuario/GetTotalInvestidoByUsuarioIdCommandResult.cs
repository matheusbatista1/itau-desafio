using InvestmentControl.Domain.Enums;

namespace InvestmentControl.Application.Commands.Results.Usuario;

public class GetTotalInvestidoByUsuarioIdCommandResult
{
    public int Id { get; set; }
    public string NomeAtivo { get; set; } = string.Empty;
    public TipoOperacao tipoOperacao { get; set; }
    public DateTime Data { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Corretagem { get; set; }
    public decimal TotalInvestido { get; set; }
}