namespace InvestmentControl.Application.Commands.Results.Posicao.Child;

public class GetClassificacaoPosicaoAndCorretagemChildCommandResult
{
    public int UsuarioId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
