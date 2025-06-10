namespace InvestmentControl.Application.Commands.Results.Posicao;

public class GetPosicaoCommandResult
{
    public int AtivoId { get; set; }
    public string AtivoNome { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal ValorTotal { get; set; }
}