using Microsoft.AspNetCore.Mvc;
using InvestmentControl.Application.Commands.Handlers.Posicao;

namespace InvestmentControl.API.Controllers.Posicao;

[ApiController]
[Route("api/posicao/")]
public class PosicoesController : ControllerBase
{
    private readonly GetClassificacaoPosicaoAndCorretagemCommandHandler _getClassificacaoPosicaoAndCorretagemCommandHandler;
    private readonly GetPosicaoCommandHandler _getPosicaoCommandHandler;

    public PosicoesController
        (
            GetClassificacaoPosicaoAndCorretagemCommandHandler getClassificacaoPosicaoAndCorretagemCommandHandler,
            GetPosicaoCommandHandler getPosicaoCommandHandler
        )
    {
        _getClassificacaoPosicaoAndCorretagemCommandHandler = getClassificacaoPosicaoAndCorretagemCommandHandler;
        _getPosicaoCommandHandler = getPosicaoCommandHandler;
    }


    #region GET

    // Retorna a classificação top 10 posições e top 10 clientes que mais pagaram corretagens
    [HttpGet("classificacao")]
    public async Task<IActionResult> GetClassificacao()
    {
        var result = await _getClassificacaoPosicaoAndCorretagemCommandHandler.HandleAsync();

        return Ok(result);
    }

    // Retorna a posicao de um usuario
    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> GetClassificacao([FromRoute] int usuarioId)
    {
        var result = await _getPosicaoCommandHandler.HandleAsync(usuarioId);

        return Ok(result);
    }

    #endregion
}