using Confluent.Kafka;
using InvestmentControl.Workers.CotacaoKafka.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly KafkaSettings _kafkaSettings;
    private IConsumer<Ignore, string>? _consumer;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IOptions<KafkaSettings> kafkaOptions)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _kafkaSettings = kafkaOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings?.BootstrapServers ?? "localhost:9092",
            GroupId = "cotacoes-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe(_kafkaSettings!.Topic);

        var circuitBreaker = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (ex, breakDelay) =>
                {
                    _logger.LogWarning($"Circuit breaker aberto por {breakDelay.TotalSeconds} segundos devido: {ex.Message}");
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker resetado, voltando ao normal.");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit breaker em estado meio aberto, testando operação.");
                });

        var fallback = Policy
            .Handle<BrokenCircuitException>()
            .FallbackAsync(async ct =>
            {
                _logger.LogWarning("Fallback acionado: serviço de cotações indisponível, ignorando processamento dessa cotação.");
                await Task.CompletedTask;
            });

        var retry = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timespan, retryCount, ctx) =>
                {
                    _logger.LogWarning($"Tentativa {retryCount} após erro: {exception.Message}. Esperando {timespan.TotalSeconds}s para nova tentativa.");
                });

        var policyWrap = Policy.WrapAsync(
            retry,
            circuitBreaker,
            fallback
        );


        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ConsumeResult<Ignore, string>? cr = null;

                try
                {
                    cr = _consumer.Consume(cancellationToken);
                    _logger.LogInformation($"Mensagem consumida: {cr.Message.Value}");
                }
                catch (ConsumeException e)
                {
                    _logger.LogError(e, "Erro ao consumir mensagem Kafka");
                    continue;
                }

                if (cr == null)
                    continue;

                using var scope = _serviceProvider.CreateScope();
                var cotacaoService = scope.ServiceProvider.GetRequiredService<CotacaoService>();

                try
                {
                    await policyWrap.ExecuteAsync(async () =>
                    {
                        await cotacaoService.ProcessarCotacaoAsync(cr.Message.Value);
                        _logger.LogInformation("Cotação processada com sucesso.");
                    });
                }
                catch (BrokenCircuitException)
                {
                    _logger.LogWarning("Circuit breaker aberto, fallback acionado. Commit da mensagem para evitar reprocessamento.");
                }
                catch (Exception ex)
                {
                    _logger.LogError( "Erro no processamento da cotação, commit da mensagem para pular e evitar loop infinito.");
                    _consumer.Commit(cr);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Execução cancelada.");
        }
        finally
        {
            _consumer?.Close();
            _consumer?.Dispose();
        }
    }
}