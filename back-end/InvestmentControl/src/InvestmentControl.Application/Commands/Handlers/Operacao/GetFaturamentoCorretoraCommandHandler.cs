using InvestmentControl.Application.Commands.Results.Operacao;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers.Operacao;

public class GetFaturamentoCorretoraCommandHandler
{
    private readonly IOperacoesRepository _operacoesRepository;

    public GetFaturamentoCorretoraCommandHandler(IOperacoesRepository operacoesRepository)
    {
        _operacoesRepository = operacoesRepository;
    }

    public async Task<GetFaturamentoCorretoraCommandResult> HandleAsync()
    {
        var operacoes = await _operacoesRepository.GetOperacoesAsync();

        var totalCorretagens = operacoes.Sum(o => o.Corretagem);
        var totalClientes = operacoes.Select(o => o.UsuarioId).Distinct().Count();
        var totalTransacoes = operacoes.Count();

        var result = new GetFaturamentoCorretoraCommandResult
        {
            TotalCorretagens = totalCorretagens,
            TotalClientes = totalClientes,
            TotalTransacoes = totalTransacoes
        };

        return result;
    }
}