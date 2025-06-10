using InvestmentControl.Domain.Enums;

namespace InvestmentControl.Domain.Entities;

public class Operacao
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public int AtivoId { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public TipoOperacao TipoOperacao { get; private set; }
    public decimal Corretagem { get; private set; }
    public DateTime DataHora { get; private set; }

    public Usuario Usuario { get; set; } = null;
    public Ativo Ativo { get; set; } = null;

    private Operacao() { }

    public Operacao(int usuarioId, int ativoId, int quantidade, decimal precoUnitario, TipoOperacao tipo, decimal corretagem)
    {
        if (quantidade <= 0 || precoUnitario <= 0)
            throw new ArgumentException("Quantidade e preço devem ser maiores que zero.");

        UsuarioId = usuarioId;
        AtivoId = ativoId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
        TipoOperacao = tipo;
        Corretagem = corretagem;
        DataHora = DateTime.UtcNow;
    }

    public decimal CalcularTotal() => Quantidade * PrecoUnitario + Corretagem;
}
