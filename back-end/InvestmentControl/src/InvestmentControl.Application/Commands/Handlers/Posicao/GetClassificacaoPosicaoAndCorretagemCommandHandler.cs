using InvestmentControl.Application.Commands.Results.Posicao;
using InvestmentControl.Application.Commands.Results.Posicao.Child;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers.Posicao;

public class GetClassificacaoPosicaoAndCorretagemCommandHandler
{
    private readonly IPosicoesRepository _posicoesRepository;
    private readonly IOperacoesRepository _operacoesRepository;

    public GetClassificacaoPosicaoAndCorretagemCommandHandler
        (
            IPosicoesRepository posicoesRepository, 
            IOperacoesRepository operacoesRepository
        )
    {
        _posicoesRepository = posicoesRepository;
        _operacoesRepository = operacoesRepository;
    }

    public async Task<GetClassificacaoPosicaoAndCorretagemCommandResult> HandleAsync()
    {
        var classificacoesPosicao = await _posicoesRepository.GetClassificacaoPosicao();
        var classificacaoCorretagemPaga = await _operacoesRepository.GetClassificacaoCorretagem();

        var classificacoesPosicaoDto = classificacoesPosicao
            .Select(c => new GetClassificacaoPosicaoAndCorretagemChildCommandResult
            {
                UsuarioId = c.UsuarioId,
                NomeCliente = c.NomeCliente,
                Valor = c.Valor
            })
            .ToList();

        var classificacoesCorretagemDto = classificacaoCorretagemPaga
            .Select(c => new GetClassificacaoPosicaoAndCorretagemChildCommandResult
            {
                UsuarioId = c.UsuarioId,
                NomeCliente = c.NomeCliente,
                Valor = c.Valor
            })
            .ToList();

        return new GetClassificacaoPosicaoAndCorretagemCommandResult
        {
            ClassificacaoPosicao = classificacoesPosicaoDto,
            ClassificacaoCorretagemPaga = classificacoesCorretagemDto
        };
    }
}