using InvestmentControl.Application.Commands.Results.Cotacao;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers.Cotacao;

public class GetUltimaCotacaoByAtivoCommandHandler
{
    private readonly ICotacoesRepository _cotacaoRepository;

    public GetUltimaCotacaoByAtivoCommandHandler(ICotacoesRepository cotacaoRepository)
    {
        _cotacaoRepository = cotacaoRepository;
    }

    public async Task<GetUltimaCotacaoByAtivoCommandResult> HandleAsync(int ativoId)
    {
        if (ativoId <= 0)
            throw new Exception("Ativo inválido");

        var cotacao = await _cotacaoRepository.GetUltimaCotacaoByAtivo(ativoId)
                      ?? throw new Exception("Não foi encontrado cotações para este ativo.");

        var result = new GetUltimaCotacaoByAtivoCommandResult
        {
            Id = cotacao.Id,
            AtivoNome = cotacao.Ativo.Nome,
            PrecoUnitario = cotacao.PrecoUnitario,
            DataHora = cotacao.DataHora
        };

        return result;
    }
}