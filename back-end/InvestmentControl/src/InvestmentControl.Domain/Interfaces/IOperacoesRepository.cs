using InvestmentControl.Domain.DTOs;
using InvestmentControl.Domain.Entities;

namespace InvestmentControl.Domain.Interfaces;

public interface IOperacoesRepository
{
    #region GET

    Task<IEnumerable<Operacao>> GetOperacoesByUsuarioIdAsync(int usuarioId);
    Task<IEnumerable<ClassificacaoDto>> GetClassificacaoCorretagem();
    Task<IEnumerable<Operacao>> GetOperacoesAsync();

    #endregion

    #region POST

    Task AddAsync(Operacao operacao);

    #endregion
}
