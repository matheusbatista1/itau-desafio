using InvestmentControl.Domain.DTOs;
using InvestmentControl.Domain.Entities;

namespace InvestmentControl.Domain.Interfaces;

public interface IPosicoesRepository
{
    #region GET

    Task<IEnumerable<ClassificacaoDto>> GetClassificacaoPosicao();
    Task<IEnumerable<Posicao>> GetByUsuarioIdAsync(int usuarioId);

    #endregion

    #region UPDATE

    Task UpdatePosicoesPorAtivoAsync(int ativoId);

    #endregion
}