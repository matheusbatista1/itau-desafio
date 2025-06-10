using InvestmentControl.Application.Commands.Results.Usuario;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers.Usuario;

public class GetTotalCorretagemByUsuarioIdCommandHandler
{
    private readonly IOperacoesRepository _operacaoRepository;

    public GetTotalCorretagemByUsuarioIdCommandHandler(IOperacoesRepository operacaoRepository)
    {
        _operacaoRepository = operacaoRepository;
    }

    public async Task<GetTotalCorretagemByUsuarioIdCommandResult> HandleAsync(int usuarioId)
    {
        var operacoes = await _operacaoRepository.GetOperacoesByUsuarioIdAsync(usuarioId);

        var totalCorretagem = operacoes.Sum(o => o.Corretagem);

        return new GetTotalCorretagemByUsuarioIdCommandResult
        {
            UsuarioId = usuarioId,
            TotalCorretagem = totalCorretagem
        };
    }
}