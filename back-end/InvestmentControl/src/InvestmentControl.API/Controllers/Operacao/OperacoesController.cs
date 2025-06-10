using InvestmentControl.Application.Commands.Inputs.Operacao;
using InvestmentControl.Application.Commands.Handlers;
using Microsoft.AspNetCore.Mvc;
using InvestmentControl.Application.Commands.Handlers.Operacao;

namespace InvestmentControl.API.Controllers.Operacao;

[ApiController]
[Route("api/operacao/")]
public class OperacoesController : ControllerBase
{
    private readonly PostOperacaoCommandHandler _createOperacaoCommandHandler;
    private readonly GetPrecoMedioCommandHandler _getPrecoMedioCotacaoCommandHandler;
    private readonly GetFaturamentoCorretoraCommandHandler _getFaturamentoCorretoraCommandHandler;

    public OperacoesController
        (
            PostOperacaoCommandHandler createOperacaoCommandHandler,
            GetPrecoMedioCommandHandler getPrecoMedioCotacaoByAtivoByUsuarioCommandHandler,
            GetFaturamentoCorretoraCommandHandler getFaturamentoCorretoraCommandHandler
        )
    {
        _createOperacaoCommandHandler = createOperacaoCommandHandler;
        _getPrecoMedioCotacaoCommandHandler = getPrecoMedioCotacaoByAtivoByUsuarioCommandHandler;
        _getFaturamentoCorretoraCommandHandler = getFaturamentoCorretoraCommandHandler;
    }


    #region GET

    // Retorna o preço médio por ativo para um úsuario
    [HttpGet("preco-medio")]
    public async Task<IActionResult> GetPrecoMedio()
    {
        var result = await _getPrecoMedioCotacaoCommandHandler.HandleAsync();

        return Ok(result);
    }

    // Retorna o faturamento da corretora com as corretagens
    [HttpGet("corretora/faturamento")]
    public async Task<IActionResult> GetFaturamentoCorretora()
    {
        var result = await _getFaturamentoCorretoraCommandHandler.HandleAsync();

        return Ok(result);
    }

    #endregion

    #region POST

    // Insere uma nova operação
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostOperacaoCommand command)
    {
        var result = await _createOperacaoCommandHandler.HandleAsync(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    #endregion
}