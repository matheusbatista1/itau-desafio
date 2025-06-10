namespace InvestmentControl.Application.Commands.Results.Operacao;

public class GetFaturamentoCorretoraCommandResult
{
    public decimal TotalCorretagens { get; set; }
    public int TotalClientes { get; set; }
    public int TotalTransacoes { get; set; }
}