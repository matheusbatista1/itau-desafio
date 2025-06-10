namespace InvestmentControl.Application.Commands.Results.Operacao;

public class GetPrecoMedioCommandResult
{
    public int AtivoId { get; set; }
    public string AtivoName { get; set; } = string.Empty;
    public decimal PrecoMedio { get; set; }
}