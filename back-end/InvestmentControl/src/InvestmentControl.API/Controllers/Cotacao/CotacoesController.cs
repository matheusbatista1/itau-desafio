using InvestmentControl.Application.Commands.Handlers.Cotacao;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentControl.API.Controllers.Cotacao;

[ApiController]
[Route("api/cotacao/")]
public class CotacoesController : ControllerBase
{
    private readonly GetUltimaCotacaoByAtivoCommandHandler _getUltimaCotacaoByAtivoCommandHandler;

    public CotacoesController
        (GetUltimaCotacaoByAtivoCommandHandler getCotacoesByAtivoCommandHandler
        )
    {
        _getUltimaCotacaoByAtivoCommandHandler = getCotacoesByAtivoCommandHandler;
    }

    #region GET

    // Retorna a ultima cotação por ativo
    [HttpGet("ativo/{ativoId}")]
    public async Task<IActionResult> GetUltimaCotacaoByAtivo([FromRoute] int ativoId)
    {
        var result = await _getUltimaCotacaoByAtivoCommandHandler.HandleAsync(ativoId);

        return Ok(result);
    }

    #endregion
}