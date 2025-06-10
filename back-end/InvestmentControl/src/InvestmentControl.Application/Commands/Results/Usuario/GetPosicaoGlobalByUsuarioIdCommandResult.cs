namespace InvestmentControl.Application.Commands.Results.Usuario;

public class GetPosicaoGlobalByUsuarioIdCommandResult
{
    public string NomeUsuario { get; set; } = string.Empty;
    public decimal TotalInvestido { get; set; }
    public decimal ValorAtualCarteira { get; set; }
    public decimal LucroPrejuizo { get; set; }
}