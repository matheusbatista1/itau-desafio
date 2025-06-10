using InvestmentControl.Application.Commands.Results.Usuario;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers.Usuario;

public class GetCotacoesByUsuarioIdCommandHandler
{
    private readonly IPosicoesRepository _posicoesRepository;
    private readonly ICotacoesRepository _cotacoesRepository;

    public GetCotacoesByUsuarioIdCommandHandler(
        IPosicoesRepository posicoesRepository,
        ICotacoesRepository cotacoesRepository)
    {
        _posicoesRepository = posicoesRepository;
        _cotacoesRepository = cotacoesRepository;
    }

    public async Task<List<GetCotacoesByUsuarioIdCommandResult>> HandleAsync(int usuarioId)
    {
        var posicoes = await _posicoesRepository.GetByUsuarioIdAsync(usuarioId);

        var ativoIds = posicoes.Select(p => p.AtivoId).Distinct().ToList();

        var cotacoes = await _cotacoesRepository.GetCotacoesAtuaisByAtivoAsync(ativoIds);

        var result = posicoes.Select(posicao =>
        {
            var precoAtual = cotacoes.TryGetValue(posicao.AtivoId, out var preco) ? preco : 0m;
            var totalInvestido = posicao.PrecoMedio * posicao.Quantidade;
            var valorMercado = precoAtual * posicao.Quantidade;
            var lucroOuPrejuizo = valorMercado - totalInvestido;

            return new GetCotacoesByUsuarioIdCommandResult
            {
                AtivoNome = posicao.Ativo.Nome,
                Quantidade = posicao.Quantidade,
                PrecoMedio = posicao.PrecoMedio,
                PrecoAtual = precoAtual,
                ValorMercado = valorMercado,
                TotalInvestido = totalInvestido,
                LucroOuPrejuizo = lucroOuPrejuizo
            };
        }).ToList();

        return result;
    }
}