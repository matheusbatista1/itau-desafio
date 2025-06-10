using InvestmentControl.Application.Commands.Results.Posicao.Child;

namespace InvestmentControl.Application.Commands.Results.Posicao;

public class GetClassificacaoPosicaoAndCorretagemCommandResult
{
    public List<GetClassificacaoPosicaoAndCorretagemChildCommandResult> ClassificacaoPosicao { get; set; } = new List<GetClassificacaoPosicaoAndCorretagemChildCommandResult>();
    public List<GetClassificacaoPosicaoAndCorretagemChildCommandResult> ClassificacaoCorretagemPaga { get; set; } = new List<GetClassificacaoPosicaoAndCorretagemChildCommandResult>();
}