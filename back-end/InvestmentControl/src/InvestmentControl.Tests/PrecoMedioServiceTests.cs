using System;
using System.Collections.Generic;
using Xunit;
using InvestmentControl.Domain.Entities;
using InvestmentControl.Domain.Enums;

namespace InvestmentControl.Tests
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
            // Arrange
            var operacoes = new List<Operacao>
            {
                new Operacao(1, 1, 10, 20m, TipoOperacao.Compra, 5m),
                new Operacao(1, 1, 20, 25m, TipoOperacao.Compra, 10m),
            };

            // Act
            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            // Assert
            // Total valor = (10*20 + 5) + (20*25 + 10) = 205 + 510 = 715
            // Total quantidade = 10 + 20 = 30
            // Preço médio = 715 / 30 = 23.8333
            Assert.Equal(23.8333m, Math.Round(precoMedio, 4));
        }

        [Fact]
        public void CalcularPrecoMedio_ComListaVazia_RetornaZero()
        {
            // Arrange
            var operacoes = new List<Operacao>();

            // Act
            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            // Assert
            Assert.Equal(0m, precoMedio);
        }

        [Fact]
        public void CalcularPrecoMedio_ComListaNula_RetornaZero()
        {
            // Act
            var precoMedio = _service.CalcularPrecoMedio(null);

            // Assert
            Assert.Equal(0m, precoMedio);
        }

        [Fact]
        public void CalcularPrecoMedio_ComOperacoesComQuantidadeZero_IgnoraOperacoesInvalidas()
        {
            // Arrange
            var operacoes = new List<Operacao>
            {
                new Operacao(1, 1, 0, 20m, TipoOperacao.Compra, 5m), // inválida (quantidade zero)
                new Operacao(1, 1, 10, 25m, TipoOperacao.Compra, 10m), // válida
            };

            // Act
            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            // Assert
            // Total valor = (10*25 + 10) = 260
            // Total quantidade = 10
            // Preço médio = 260 / 10 = 26
            Assert.Equal(26m, precoMedio);
        }

        [Fact]
        public void CalcularPrecoMedio_ComSomenteOperacoesDeVenda_RetornaZero()
        {
            // Arrange
            var operacoes = new List<Operacao>
            {
                new Operacao(1, 1, 10, 20m, TipoOperacao.Venda, 5m),
                new Operacao(1, 1, 15, 25m, TipoOperacao.Venda, 10m),
            };

            // Act
            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            // Assert
            Assert.Equal(0m, precoMedio);
        }

        [Fact]
        public void CalcularPrecoMedio_ComPrecoUnitarioZero_IgnoraOperacoesInvalidas()
        {
            // Arrange
            var operacoes = new List<Operacao>
            {
                new Operacao(1, 1, 10, 0m, TipoOperacao.Compra, 5m), // inválida (preço zero)
                new Operacao(1, 1, 5, 30m, TipoOperacao.Compra, 2m),  // válida
            };

            // Act
            var precoMedio = _service.CalcularPrecoMedio(operacoes);

            // Assert
            // Total valor = (5 * 30 + 2) = 152
            // Total quantidade = 5
            // Preço médio = 152 / 5 = 30.4
            Assert.Equal(30.4m, precoMedio);
        }
    }
}