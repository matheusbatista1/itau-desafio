using InvestmentControl.Application.Commands.Results.Usuario;
using InvestmentControl.Domain.Enums;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers.Usuario;

public class GetPosicaoGlobalByUsuarioIdCommandHandler
{
    private readonly IOperacoesRepository _operacaoRepository;
    private readonly ICotacoesRepository _cotacaoRepository;

    public GetPosicaoGlobalByUsuarioIdCommandHandler(IOperacoesRepository operacaoRepository, ICotacoesRepository cotacaoRepository)
    {
        _operacaoRepository = operacaoRepository;
        _cotacaoRepository = cotacaoRepository;
    }

    public async Task<GetPosicaoGlobalByUsuarioIdCommandResult> HandleAsync(int usuarioId)
    {
        var operacoes = await _operacaoRepository.GetOperacoesByUsuarioIdAsync(usuarioId) ?? throw new Exception("Não foram econtradas operações para esse úsuario");
        var usuario = operacoes.FirstOrDefault()!.Usuario;
        var ativosIds = operacoes.Select(o => o.AtivoId).Distinct();
        var precosAtuais = new Dictionary<int, decimal>();

        foreach (var ativoId in ativosIds)
        {
            var cotacao = await _cotacaoRepository.GetCotacaoAtual(ativoId);
            if (cotacao != null)
                precosAtuais[ativoId] = cotacao.PrecoUnitario;
        }

        decimal totalInvestido = 0;
        decimal valorAtualCarteira = 0;

        var ativosAgrupados = operacoes.GroupBy(o => o.AtivoId);

        foreach (var grupo in ativosAgrupados)
        {
            var compras = grupo.Where(o => o.TipoOperacao == TipoOperacao.Compra);
            var vendas = grupo.Where(o => o.TipoOperacao == TipoOperacao.Venda);

            var quantidadeComprada = compras.Sum(o => o.Quantidade);
            var totalComprado = compras.Sum(o => (o.Quantidade * o.PrecoUnitario) + o.Corretagem);

            var quantidadeVendida = vendas.Sum(o => o.Quantidade);
            var quantidadeAtual = quantidadeComprada - quantidadeVendida;

            if (quantidadeAtual <= 0)
                continue;

            precosAtuais.TryGetValue(grupo.Key, out var precoAtual);

            totalInvestido += totalComprado;
            valorAtualCarteira += precoAtual * quantidadeAtual;
        }

        return new GetPosicaoGlobalByUsuarioIdCommandResult
        {
            NomeUsuario = usuario.Nome,
            TotalInvestido = totalInvestido,
            ValorAtualCarteira = valorAtualCarteira,
            LucroPrejuizo = valorAtualCarteira - totalInvestido
        };
    }
}