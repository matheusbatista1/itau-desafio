namespace InvestmentControl.Domain.Entities;

public class Posicao
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public int AtivoId { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoMedio { get; private set; }
    public decimal PL { get; private set; }

    public Usuario Usuario { get; set; } = null;
    public Ativo Ativo { get; set; } = null;

    public Posicao() { }

    public Posicao(int usuarioId, int ativoId, int quantidade, decimal precoMedio, decimal pl)
    {
        if (quantidade < 0 || precoMedio < 0)
            throw new ArgumentException("Quantidade e preço médio devem ser não-negativos.");

        UsuarioId = usuarioId;
        AtivoId = ativoId;
        Quantidade = quantidade;
        PrecoMedio = precoMedio;
        PL = pl;
    }

    public void AtualizarPL(decimal novaCotacao)
    {
        PL = (novaCotacao - PrecoMedio) * Quantidade;
    }
}