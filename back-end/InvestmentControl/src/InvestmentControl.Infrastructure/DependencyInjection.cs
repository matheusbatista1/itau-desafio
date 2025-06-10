using InvestmentControl.Domain.Interfaces;
using InvestmentControl.Infrastructure.Data;
using InvestmentControl.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvestmentControl.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
        options.UseMySQL(config.GetConnectionString("DefaultConnection")!));

        services.AddScoped<IOperacoesRepository, OperacaoRepository>();
        services.AddScoped<ICotacoesRepository, CotacoesRepository>();
        services.AddScoped<IPosicoesRepository, PosicoesRepository>();
        services.AddScoped<IUsuariosRepository, UsuariosRepository>();

        return services;
    }
}