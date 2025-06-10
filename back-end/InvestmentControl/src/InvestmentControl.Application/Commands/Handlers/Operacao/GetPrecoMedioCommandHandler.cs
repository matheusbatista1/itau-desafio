using InvestmentControl.Application.Commands.Results.Operacao;
using InvestmentControl.Domain.Interfaces;
using InvestmentControl.Domain.Services;

namespace InvestmentControl.Application.Commands.Handlers.Operacao;

public class GetPrecoMedioCommandHandler
{
    private readonly IOperacoesRepository _operacaoRepository;
    private readonly PrecoMedioService _precoMedioService;

    public GetPrecoMedioCommandHandler
        (
            IOperacoesRepository operacaoRepository,
            PrecoMedioService precoMedioService
        )
    {
        _operacaoRepository = operacaoRepository;
        _precoMedioService = precoMedioService;
    }

    public async Task<List<GetPrecoMedioCommandResult>> HandleAsync()
    {
        var operacoes = await _operacaoRepository.GetOperacoesAsync();

        var grupos = operacoes.GroupBy(o => new { o.AtivoId, o.Ativo.Nome });

        var resultado = new List<GetPrecoMedioCommandResult>();

        foreach (var grupo in grupos)
        {
            var precoMedio = _precoMedioService.CalcularPrecoMedio(grupo.ToList());

            resultado.Add(new GetPrecoMedioCommandResult
            {
                AtivoId = grupo.Key.AtivoId,
                AtivoName = grupo.Key.Nome,
                PrecoMedio = precoMedio
            });
        }

        return resultado;
    }
}