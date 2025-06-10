using InvestmentControl.Application;
using InvestmentControl.Infrastructure;
using InvestmentControl.Infrastructure.Data;
using InvestmentControl.Workers.CotacaoKafka.Settings;
using Microsoft.EntityFrameworkCore;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<KafkaSettings>(hostContext.Configuration.GetSection("Kafka"));
        services.AddHostedService<Worker>();
        services.AddInfrastructure(hostContext.Configuration);
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(hostContext.Configuration.GetConnectionString("DefaultConnection")!));
        services.AddApplication();
        services.AddScoped<CotacaoService>();
    })
    .Build()
    .Run();