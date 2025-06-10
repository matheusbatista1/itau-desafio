namespace InvestmentControl.Application.Commands.Results.Cotacao;

public class GetUltimaCotacaoByAtivoCommandResult
{
    public int Id { get; set; }
    public string AtivoNome { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public DateTime DataHora { get; set; }
}
