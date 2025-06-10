namespace InvestmentControl.Application.Commands.Results.Usuario;

public class GetCotacoesByUsuarioIdCommandResult
{
    public string AtivoNome { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal PrecoMedio { get; set; }
    public decimal PrecoAtual { get; set; }
    public decimal ValorMercado { get; set; }
    public decimal TotalInvestido { get; set; }
    public decimal LucroOuPrejuizo { get; set; }
}
