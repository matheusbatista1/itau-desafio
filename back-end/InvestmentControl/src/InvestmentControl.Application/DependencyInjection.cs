using InvestmentControl.Application.Commands.Handlers;
using InvestmentControl.Application.Commands.Handlers.Cotacao;
using InvestmentControl.Application.Commands.Handlers.Operacao;
using InvestmentControl.Application.Commands.Handlers.Posicao;
using InvestmentControl.Application.Commands.Handlers.Usuario;
using InvestmentControl.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvestmentControl.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<PostOperacaoCommandHandler>();
        services.AddScoped<GetUltimaCotacaoByAtivoCommandHandler>();

        #region USUARIO

            services.AddScoped<GetTotalInvestidoByUsuarioIdCommandHandler>(); 
            services.AddScoped<GetPosicaoGlobalByUsuarioIdCommandHandler>();
            services.AddScoped<GetTotalCorretagemByUsuarioIdCommandHandler>(); 
            services.AddScoped<GetCotacoesByUsuarioIdCommandHandler>();
            services.AddScoped<GetUsuarioIdCommandHandler>();

        #endregion

        #region OPERACAO

        services.AddScoped<GetPrecoMedioCommandHandler>();
            services.AddScoped<GetFaturamentoCorretoraCommandHandler>();
            services.AddScoped<PostOperacaoCommandHandler>();

        #endregion

        #region COTACAO

            services.AddScoped<GetUltimaCotacaoByAtivoCommandHandler>();

        #endregion

        #region POSICAO
        
            services.AddScoped<GetClassificacaoPosicaoAndCorretagemCommandHandler>(); 
            services.AddScoped<GetPosicaoCommandHandler>();

        #endregion

        #region SERVICES

        services.AddScoped<PrecoMedioService>();

        #endregion

        return services;
    }
}