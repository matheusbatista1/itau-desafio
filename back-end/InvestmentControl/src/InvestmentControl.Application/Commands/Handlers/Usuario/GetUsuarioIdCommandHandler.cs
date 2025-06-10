using InvestmentControl.Application.Commands.Results.Usuario;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers.Usuario;

public class GetUsuarioIdCommandHandler
{
    private readonly IUsuariosRepository _usuarioRepository;

    public GetUsuarioIdCommandHandler(IUsuariosRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<GetUsuarioIdCommandResult?> HandleAsync(string email)
    {
        var usuario = await _usuarioRepository.GetUsuarioIdByEmailAsync(email);

        if (usuario is null)
            return null;

        return new GetUsuarioIdCommandResult
        {
            Id = usuario.Id,
            Nome = usuario.Nome
        };
    }
}