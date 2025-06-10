using InvestmentControl.Domain.Interfaces;
using System.Text.Json;

public class CotacaoService
{
    private readonly ICotacoesRepository _cotacoesRepository;
    private readonly IPosicoesRepository _posicoesRepository;
    private readonly ILogger<CotacaoService> _logger;

    public CotacaoService
        (
            ICotacoesRepository repository,
            IPosicoesRepository posicoesRepository,
            ILogger<CotacaoService> logger
        )
    {
        _cotacoesRepository = repository;
        _posicoesRepository = posicoesRepository; ;
        _logger = logger;
    }

    public async Task ProcessarCotacaoAsync(string mensagemJson)
    {
        _logger.LogInformation($"Mensagem JSON recebida: {mensagemJson}");

        var cotacao = JsonSerializer.Deserialize<Cotacao>(mensagemJson);
        if (cotacao == null)
        {
            _logger.LogError("Falha na desserialização: cotação é nula.");
            throw new Exception("Mensagem inválida");
        }

        _logger.LogInformation($"Cotação desserializada: AtivoId={cotacao.AtivoId}, PrecoUnitario={cotacao.PrecoUnitario}, DataHora={cotacao.DataHora}, Ativo={(cotacao.Ativo != null ? cotacao.Ativo.Id : "null")}");

        var cotacaoExistente = await _cotacoesRepository.GetCotacaoByUniqueKey(cotacao.AtivoId, cotacao.DataHora);
        if (cotacaoExistente != null)
        {
            _logger.LogInformation("Cotação já existe, ignorando.");
            return;
        }

        cotacao.Ativo = null!;
        await _cotacoesRepository.SalvarAsync(cotacao);
        await _posicoesRepository.UpdatePosicoesPorAtivoAsync(cotacao.AtivoId);
    }
}