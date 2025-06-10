using InvestmentControl.Domain.DTOs;
using InvestmentControl.Domain.Entities;
using InvestmentControl.Domain.Interfaces;
using InvestmentControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestmentControl.Infrastructure.Repositories;

public class OperacaoRepository : IOperacoesRepository
{
    private readonly AppDbContext _ctx;

    public OperacaoRepository(AppDbContext context)
    {
        _ctx = context;
    }

    #region GET

    public async Task<IEnumerable<Operacao>> GetOperacoesByUsuarioIdAsync(int usuarioId)
    {
        return await _ctx.Operacoes
            .Include(o => o.Ativo)
            .Include(o => o.Usuario)
            .Where(o => o.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Operacao>> GetOperacoesAsync()
    {
        return await _ctx.Operacoes.Include(o => o.Ativo).ToListAsync();
    }

    public async Task<IEnumerable<ClassificacaoDto>> GetClassificacaoCorretagem()
    {
        var result = await _ctx.Operacoes
            .Include(o => o.Usuario)
            .GroupBy(o => o.Usuario)
            .Select(g => new ClassificacaoDto
            {
                UsuarioId = g.Key.Id,
                NomeCliente = g.Key.Nome,
                Valor = g.Sum(o => o.Corretagem)
            })
            .OrderByDescending(x => x.Valor)
            .Take(10)
            .ToListAsync();

        return result.Cast<ClassificacaoDto>();
    }

    #endregion

    #region POST

    public async Task AddAsync(Operacao operacao)
    {
        await _ctx.Operacoes.AddAsync(operacao);
        await _ctx.SaveChangesAsync();
    }

    #endregion
}
