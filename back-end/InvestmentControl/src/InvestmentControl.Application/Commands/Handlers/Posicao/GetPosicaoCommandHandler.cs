using InvestmentControl.Application.Commands.Results.Posicao.Child;
using InvestmentControl.Application.Commands.Results.Posicao;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers.Posicao;

public class GetPosicaoCommandHandler
{
    private readonly IPosicoesRepository _posicoesRepository;

    public GetPosicaoCommandHandler
        (
            IPosicoesRepository posicoesRepository
        )
    {
        _posicoesRepository = posicoesRepository;
    }

    public async Task<IEnumerable<GetPosicaoCommandResult>> HandleAsync(int usuarioId)
    {
        var posicoes = await _posicoesRepository.GetByUsuarioIdAsync(usuarioId);

        return posicoes.Select(p => new GetPosicaoCommandResult
        {
            AtivoId = p.AtivoId,
            AtivoNome = p.Ativo.Nome,
            Quantidade = p.Quantidade,
            ValorTotal = p.Quantidade * p.PrecoMedio
        });
    }
}