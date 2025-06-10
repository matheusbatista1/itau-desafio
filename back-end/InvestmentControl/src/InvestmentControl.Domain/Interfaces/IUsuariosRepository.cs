using InvestmentControl.Domain.Entities;

namespace InvestmentControl.Domain.Interfaces;

public interface IUsuariosRepository
{
    #region GET

    Task<Usuario?> GetUsuarioIdByEmailAsync(string email);    

    #endregion
}