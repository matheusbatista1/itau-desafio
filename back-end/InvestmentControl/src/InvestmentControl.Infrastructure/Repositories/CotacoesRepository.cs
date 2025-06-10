using InvestmentControl.Domain.Entities;
using InvestmentControl.Domain.Interfaces;
using InvestmentControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestmentControl.Infrastructure.Repositories;

public class CotacoesRepository : ICotacoesRepository
{
    private readonly AppDbContext _ctx;

    public CotacoesRepository(AppDbContext context)
    {
        _ctx = context;
    }

    #region GET

    public async Task<Cotacao?> GetCotacaoAtual(int ativoId)
    {
        return await _ctx.Cotacoes
            .Where(c => c.AtivoId == ativoId)
            .OrderByDescending(c => c.DataHora)
            .FirstOrDefaultAsync();
    }

    public async Task<Cotacao?> GetUltimaCotacaoByAtivo(int ativoId)
    {
        return await _ctx.Cotacoes
            .Where(c => c.AtivoId == ativoId)
            .Include(c => c.Ativo)
            .OrderByDescending(c => c.DataHora)
            .FirstOrDefaultAsync();
    }

    public async Task<Cotacao?> GetCotacaoByUniqueKey(int ativoId, DateTime data)
    {
        return await _ctx.Cotacoes
            .FirstOrDefaultAsync(c => c.AtivoId == ativoId && c.DataHora == data);
    }

    public async Task<Dictionary<int, decimal>> GetCotacoesAtuaisByAtivoAsync(List<int> ativoIds)
    {
        return await _ctx.Cotacoes
            .Where(c => ativoIds.Contains(c.AtivoId))
            .GroupBy(c => c.AtivoId)
            .Select(g => g.OrderByDescending(c => c.DataHora).First())
            .ToDictionaryAsync(c => c.AtivoId, c => c.PrecoUnitario);
    }

    #endregion

    #region POST

    public async Task SalvarAsync(Cotacao cotacao)
    {
        _ctx.Cotacoes.Add(cotacao);
        await _ctx.SaveChangesAsync();
    }

    #endregion
}
