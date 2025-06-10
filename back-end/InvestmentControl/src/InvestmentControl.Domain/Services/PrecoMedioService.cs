using InvestmentControl.Domain.Entities;
using InvestmentControl.Domain.Enums;

namespace InvestmentControl.Domain.Services;

public class PrecoMedioService
{
    public decimal CalcularPrecoMedio(List<Operacao>? operacoes)
    {
        if (operacoes == null || operacoes.Count == 0)
            return 0;

        var comprasValidas = operacoes
            .Where(o => o.TipoOperacao == TipoOperacao.Compra
                        && o.Quantidade > 0
                        && o.PrecoUnitario > 0)
            .ToList();

        if (comprasValidas.Count == 0)
            return 0;

        decimal totalQuantidade = comprasValidas.Sum(o => o.Quantidade);

        if (totalQuantidade == 0)
            return 0;

        decimal totalValor = comprasValidas.Sum(o => (o.PrecoUnitario * o.Quantidade) + o.Corretagem);

        decimal precoMedio = totalValor / totalQuantidade;

        return precoMedio < 0 ? 0 : precoMedio;
    }
}