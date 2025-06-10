using InvestmentControl.Domain.Entities;
using InvestmentControl.Domain.Enums;
using InvestmentControl.Domain.Services;

namespace InvestmentContro.Tests.Services
{
    public class PrecoMedioServiceTests
    {
        private readonly PrecoMedioService _service;

        public PrecoMedioServiceTests()
        {
            _service = new PrecoMedioService();
        }

        [Fact]
        public void CalcularPrecoMedio_ComOperacoesValidas_RetornaPrecoMedioCorreto()
        {
            var operacoes = new List<Operacao>
            {
                new Operacao(1, 1, 10, 20m, TipoOperacao.Compra, 5m),
                new Operacao(1, 1, 20, 25m, TipoOperacao.Compra, 10m),
            };

            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            Assert.Equal(23.8333m, Math.Round(precoMedio, 4));
        }

        [Fact]
        public void CalcularPrecoMedio_ComListaVazia_RetornaZero()
        {
            var operacoes = new List<Operacao>();

            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            Assert.Equal(0m, precoMedio);
        }

        [Fact]
        public void CalcularPrecoMedio_ComListaNula_RetornaZero()
        {
            var precoMedio = _service.CalcularPrecoMedio(null);

            Assert.Equal(0m, precoMedio);
        }

        [Fact]
        public void CalcularPrecoMedio_ComOperacoesIgnorandoInvalidas_RetornaPrecoMedioComOperacoesValidas()
        {
            var operacoes = new List<Operacao>
            {
                new Operacao(1, 1, 10, 25m, TipoOperacao.Compra, 10m),
            };

            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            Assert.Equal(26m, precoMedio);
        }

        [Fact]
        public void CalcularPrecoMedio_ComSomenteOperacoesDeVenda_RetornaZero()
        {
            var operacoes = new List<Operacao>
            {
                new Operacao(1, 1, 10, 20m, TipoOperacao.Venda, 5m),
                new Operacao(1, 1, 15, 25m, TipoOperacao.Venda, 10m),
            };

            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            Assert.Equal(0m, precoMedio);
        }

        [Fact]
        public void CalcularPrecoMedio_ComOperacoesComPrecoUnitarioIgnorandoInvalidas_RetornaPrecoMedioComOperacoesValidas()
        {
            var operacoes = new List<Operacao>
            {
                new Operacao(1, 1, 5, 30m, TipoOperacao.Compra, 2m),
            };

            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            Assert.Equal(30.4m, precoMedio);
        }
    }
}