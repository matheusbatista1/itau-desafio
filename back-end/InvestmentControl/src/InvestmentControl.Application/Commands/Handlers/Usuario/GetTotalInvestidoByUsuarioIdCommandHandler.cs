using InvestmentControl.Application.Commands.Results.Usuario;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers.Usuario;

public class GetTotalInvestidoByUsuarioIdCommandHandler
{
    private readonly IOperacoesRepository _operacaoRepository;

    public GetTotalInvestidoByUsuarioIdCommandHandler(IOperacoesRepository operacaoRepository)
    {
        _operacaoRepository = operacaoRepository;
    }

    public async Task<List<GetTotalInvestidoByUsuarioIdCommandResult>> HandleAsync(int usuarioId)
    {
        if (usuarioId <= 0)
            throw new Exception("Usuario inválido");

        var operacoes = await _operacaoRepository.GetOperacoesByUsuarioIdAsync(usuarioId);

        var result = operacoes
            .GroupBy(o => new { o.AtivoId, o.Ativo.Nome, o.Corretagem, o.TipoOperacao, o.PrecoUnitario, o.DataHora, o.Quantidade })
            .Select(g => new GetTotalInvestidoByUsuarioIdCommandResult
            {
                Id = g.Key.AtivoId,
                NomeAtivo = g.Key.Nome,
                Corretagem = g.Key.Corretagem,
                PrecoUnitario = g.Key.PrecoUnitario,
                tipoOperacao = g.Key.TipoOperacao,
                Data = g.Key.DataHora,
                Quantidade = g.Key.Quantidade,
                TotalInvestido = g.Sum(o => o.CalcularTotal())
            })
            .ToList();

        return result;
    }
}