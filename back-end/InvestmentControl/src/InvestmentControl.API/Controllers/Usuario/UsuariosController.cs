using InvestmentControl.Application.Commands.Handlers.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentControl.API.Controllers.Usuario;

[ApiController]
[Route("api/usuario/")]
public class UsuariosController : ControllerBase
{
    private readonly GetTotalInvestidoByUsuarioIdCommandHandler _getTotalInvestidoByUsuarioIdCommandHandler;
    private readonly GetPosicaoGlobalByUsuarioIdCommandHandler _getPosicaoGlobalByUsuarioIdCommandHandler;
    private readonly GetTotalCorretagemByUsuarioIdCommandHandler _getTotalCorretagemByUsuarioIdCommandHandler;
    private readonly GetCotacoesByUsuarioIdCommandHandler _getCotacoesByUsuarioIdCommandHandler;
    private readonly GetUsuarioIdCommandHandler _getUsuarioIdCommandHandler;

    public UsuariosController
        (
            GetTotalInvestidoByUsuarioIdCommandHandler getTotalInvestidoByUsuarioIdCommandHandler, 
            GetPosicaoGlobalByUsuarioIdCommandHandler getPosicaoGlobalByUsuarioIdCommandHandler,
            GetTotalCorretagemByUsuarioIdCommandHandler getTotalCorretagemByUsuarioIdCommandHandler,
            GetCotacoesByUsuarioIdCommandHandler getCotacoesByUsuarioIdCommandHandler,
            GetUsuarioIdCommandHandler getUsuarioIdCommandHandler

        )
    {
        _getTotalInvestidoByUsuarioIdCommandHandler = getTotalInvestidoByUsuarioIdCommandHandler;
        _getPosicaoGlobalByUsuarioIdCommandHandler = getPosicaoGlobalByUsuarioIdCommandHandler;
        _getTotalCorretagemByUsuarioIdCommandHandler = getTotalCorretagemByUsuarioIdCommandHandler;
        _getCotacoesByUsuarioIdCommandHandler = getCotacoesByUsuarioIdCommandHandler;
        _getUsuarioIdCommandHandler = getUsuarioIdCommandHandler;
    }


    #region GET

    // Retorna todas operações de um úsuario
    [HttpGet("{usuarioId}/operacoes")]
    public async Task<IActionResult> GetOperacoesByUsuarioId([FromRoute] int usuarioId)
    {
        var result = await _getTotalInvestidoByUsuarioIdCommandHandler.HandleAsync(usuarioId);

        return Ok(result);
    }

    // Retorna a posição global de um úsuario
    [HttpGet("{usuarioId}/global")]
    public async Task<IActionResult> GetPosicaoGlobalByUsuarioId([FromRoute] int usuarioId)
    {
        var result = await _getPosicaoGlobalByUsuarioIdCommandHandler.HandleAsync(usuarioId);

        return Ok(result);
    }

    // Retorna o total de corretagem de um úsuario
    [HttpGet("{usuarioId}/corretagem")]
    public async Task<IActionResult> GetTotalCorretagemByUsuarioId([FromRoute] int usuarioId)
    {
        var result = await _getTotalCorretagemByUsuarioIdCommandHandler.HandleAsync(usuarioId);

        return Ok(result);
    }

    // Retorna as cotacoes de todos ativos do úsuario
    [HttpGet("{usuarioId}/cotacoes")]
    public async Task<IActionResult> GetCotacoesByUsuarioId([FromRoute] int usuarioId)
    {
        var result = await _getCotacoesByUsuarioIdCommandHandler.HandleAsync(usuarioId);

        return Ok(result);
    }

    // Retorna o Id para consultas atraves do email informado
    [HttpGet("{email}")]
    public async Task<IActionResult> GetUsuarioId([FromRoute] string email)
    {
        var result = await _getUsuarioIdCommandHandler.HandleAsync(email);

        return Ok(result);
    }

    #endregion
}