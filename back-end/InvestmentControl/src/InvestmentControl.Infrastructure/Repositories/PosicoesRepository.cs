using InvestmentControl.Domain.DTOs;
using InvestmentControl.Domain.Entities;
using InvestmentControl.Domain.Interfaces;
using InvestmentControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestmentControl.Infrastructure.Repositories;

public class PosicoesRepository : IPosicoesRepository
{
    private readonly AppDbContext _ctx;

    public PosicoesRepository(AppDbContext context)
    {
        _ctx = context;
    }
    #region GET

    public async Task<IEnumerable<ClassificacaoDto>> GetClassificacaoPosicao()
    {
        var top10 = await _ctx.Posicoes
            .Include(p => p.Usuario)
            .GroupBy(p => p.Usuario)
            .Select(g => new ClassificacaoDto
            {
                UsuarioId = g.Key.Id,
                NomeCliente = g.Key.Nome,
                Valor = g.Sum(p => p.Quantidade * p.PrecoMedio)
            })
            .OrderByDescending(x => x.Valor)
            .Take(10)
            .ToListAsync();

        return top10;
    }

    public async Task<IEnumerable<Posicao>> GetByUsuarioIdAsync(int usuarioId)
    {
        return await _ctx.Posicoes
            .Include(p => p.Ativo)
            .Where(p => p.UsuarioId == usuarioId)
            .ToListAsync();
    }

    #endregion

    #region UPDATE

    public async Task UpdatePosicoesPorAtivoAsync(int ativoId)
    {
        var cotacaoAtual = await _ctx.Cotacoes
            .Where(c => c.AtivoId == ativoId)
            .OrderByDescending(c => c.DataHora)
            .Select(c => c.PrecoUnitario)
            .FirstOrDefaultAsync();

        if (cotacaoAtual == 0)
            return;

        var posicoes = await _ctx.Posicoes
            .Where(p => p.AtivoId == ativoId)
            .ToListAsync();

        foreach (var posicao in posicoes)
        {
            posicao.AtualizarPL(cotacaoAtual);
        }

        await _ctx.SaveChangesAsync();
    }

    #endregion
}
