using InvestmentControl.Domain.Entities;
using InvestmentControl.Domain.Interfaces;
using InvestmentControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestmentControl.Infrastructure.Repositories;

public class UsuariosRepository : IUsuariosRepository
{
    private readonly AppDbContext _ctx;

    public UsuariosRepository(AppDbContext context)
    {
        _ctx = context;
    }

    #region GET
    public async Task<Usuario?> GetUsuarioIdByEmailAsync(string email)
    {
        return await _ctx.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    #endregion
}
