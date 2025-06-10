using InvestmentControl.Domain.Enums;

namespace InvestmentControl.Application.Commands.Inputs.Operacao;

public class PostOperacaoCommand
{
    public int   UsuarioId { get; set; }
    public int AtivoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public TipoOperacao TipoOperacao { get; set; }
    public decimal Corretagem { get; set; }
}