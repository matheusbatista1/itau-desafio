namespace InvestmentControl.Domain.Interfaces;

public interface ICotacoesRepository
{
    #region GET

    Task<Cotacao?> GetUltimaCotacaoByAtivo(int ativoId);
    Task<Cotacao?> GetCotacaoAtual(int ativoId);
    Task<Cotacao?> GetCotacaoByUniqueKey(int ativoId, DateTime data);
    Task<Dictionary<int, decimal>> GetCotacoesAtuaisByAtivoAsync(List<int> ativoIds);

    #endregion

    #region POST

    Task SalvarAsync(Cotacao cotacao);

    #endregion
}