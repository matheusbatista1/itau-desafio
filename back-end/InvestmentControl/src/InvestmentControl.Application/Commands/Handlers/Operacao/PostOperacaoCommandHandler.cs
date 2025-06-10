using InvestmentControl.Application.Commands.Inputs.Operacao;
using InvestmentControl.Application.Commands.Results;
using Entities = InvestmentControl.Domain.Entities;
using InvestmentControl.Domain.Interfaces;

namespace InvestmentControl.Application.Commands.Handlers;

public class PostOperacaoCommandHandler
{
    private readonly IOperacoesRepository _operacaoRepository;

    public PostOperacaoCommandHandler(IOperacoesRepository operacaoRepository)
    {
        _operacaoRepository = operacaoRepository;
    }

    public async Task<CommandResult> HandleAsync(PostOperacaoCommand cmd)
    {
        if (cmd.Quantidade <= 0 || cmd.PrecoUnitario <= 0)
            return CommandResult.Fail("Quantidade e preço devem ser maiores que zero.");

        var operacao = new Entities.Operacao(
            cmd.UsuarioId,
            cmd.AtivoId,
            cmd.Quantidade,
            cmd.PrecoUnitario,
            cmd.TipoOperacao,
            cmd.Corretagem
        );

        await _operacaoRepository.AddAsync(operacao);

        return CommandResult.Ok("Operação criada com sucesso", new { operacao.Id });
    }
}